using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Infrastructure.Enums;
using PruebaTecnica.Infrastructure.Utility;
using System.Collections;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace PruebaTecnica.Infrastructure.RepositoryCore
{
    public class DataSource : IDisposable, IDataSource
    {
        private readonly IConfiguration _configuration;
        private string _name = string.Empty;
        private bool disposed = false;
        private readonly ConcurrentDictionary<string, IDbConnection> _DictCon = new(concurrencyLevel: 4 * Environment.ProcessorCount,
                                                                                     capacity: 1,
                                                                                     comparer: StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDbConnection> _DictBD = new(concurrencyLevel: 4 * Environment.ProcessorCount,
                                                                                     capacity: 1,
                                                                                     comparer: StringComparer.OrdinalIgnoreCase);
        public DataSource(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.CreateDataSource();
        }

        private void CreateDataSource()
        {
            var section = this._configuration.GetSection($"ConnectionStringWrapperSettings");
            var lsConnections = section?.GetSection("ConnectionStringEntries").GetChildren();
            if (lsConnections != null)
            {
                foreach (IConfigurationSection configurationSection in lsConnections)
                {
                    string name = configurationSection["Name"] ?? string.Empty;
                    string BDDefault = configurationSection["BaseDatosDefault"] ?? string.Empty;
                    string ConnectionString = configurationSection["ConnectionString"] ?? string.Empty;
                    string ProviderName = configurationSection["ProviderName"] ?? string.Empty;

                    Initialize(name, BDDefault, ConnectionString, ProviderName);
                }
            }
        }

        private async void Initialize(string name, string BDDefault, string connectionString, string providerName)
        {
            DbProviderFactory? _factory;
            var _connectionString = string.Format(connectionString, BDDefault);

            try
            {
                _factory = DbProviderFactories.GetFactory(providerName);
            }
            catch
            {
                DbProviderFactories.RegisterFactory(providerName, SqlClientFactory.Instance);
                _factory = DbProviderFactories.GetFactory(providerName);
            }

            var _connectionCurrent = _factory.CreateConnection();
            if (_connectionCurrent != null)
            {
                _connectionCurrent.ConnectionString = _connectionString;
                _DictCon.TryAdd(name.ToUpper(), _connectionCurrent);
                _DictBD.TryAdd(string.Concat(name.ToUpper(), "-", BDDefault.ToUpper()), _connectionCurrent);
                await _connectionCurrent.OpenAsync();
            }
        }

        private async Task<IDbConnection?> GetNewConnection(string serverDB, string database)
        {
            //var tcs = new TaskCompletionSource<IDbConnection>(TaskCreationOptions.RunContinuationsAsynchronously);
            IDbConnection? conn;
            var _serverDB = string.Concat(serverDB.ToUpper(), "-", database.ToUpper());
            bool containCon = _DictBD.ContainsKey(_serverDB);

            if (containCon)
            {
                _DictBD.TryGetValue(_serverDB, out conn);
            }
            else
            {
                _DictCon.TryGetValue(serverDB.ToUpper(), out conn);
                if (conn is ICloneable IClonConnection)
                {
                    conn = IClonConnection.Clone() as IDbConnection;
                    if (conn != null)
                    {
                        _DictBD.TryAdd(_serverDB, conn);
                    }
                }
            }

            if (conn != null)
            {
                if (conn.State == ConnectionState.Closed)
                {
                    if (conn is DbConnection _DbConnection)
                    {
                        await _DbConnection.OpenAsync();
                        if (!containCon)
                        {
                            conn.ChangeDatabase(database);
                        }
                        _name = _serverDB;
                    }
                }
            }

            return conn;
        }

        private async Task<IDictionary> ExecuteQueryMultipleAsync<T>(ParameterServer<T> parameterServer)
        {
            var data = new Dictionary<string, IEnumerable>();
            var _conn = await GetNewConnection(parameterServer.NombreServer!, parameterServer.NombreBase!);
            var _multi = await _conn!.QueryMultipleAsync(parameterServer.QuerySP!, parameterServer.Parameters, commandType: parameterServer.TipoComando);

            if (_multi != null)
            {
                foreach (var item in parameterServer.Tablas!)
                {
                    data.Add(item, _multi.Read().ToList());
                }
            }

            return data;
        }

        private async Task<IEnumerable<T>> ExecuteQueryListAsync<T>(ParameterServer<T> parameterServer)
        {
            var _conn = await GetNewConnection(parameterServer.NombreServer!, parameterServer.NombreBase!);
            var _result = await _conn!.QueryAsync<T>(parameterServer.QuerySP!, parameterServer.Parameters, commandType: parameterServer.TipoComando);

            return _result;
        }

        private async Task<T> ExecuteQuerySingleAsync<T>(ParameterServer<T> parameterServer)
        {
            var _conn = await GetNewConnection(parameterServer.NombreServer!, parameterServer.NombreBase!);
            var _result = await _conn!.QuerySingleAsync<T>(parameterServer.QuerySP!, parameterServer.Parameters, commandType: parameterServer.TipoComando);

            return _result;
        }

        private async Task<dynamic?> ExecuteQueryNoReturnAsync<T>(ParameterServer<T> parameterServer)
        {
            parameterServer.TipoComando = CommandType.Text;

            return await ExecuteQuerySPNRAsync(parameterServer);
        }

        private async Task<dynamic?> ExecuteQuerySPAsync<T>(ParameterServer<T> parameterServer)
        {
            parameterServer.TipoComando = CommandType.StoredProcedure;

            return await ExecuteQuerySPNRAsync(parameterServer);
        }

        private async Task<dynamic?> ExecuteQuerySPNRAsync<T>(ParameterServer<T> parameterServer)
        {
            var _conn = await GetNewConnection(parameterServer.NombreServer!, parameterServer.NombreBase!);
            var _result = await _conn!.ExecuteAsync(parameterServer.QuerySP!, parameterServer.Parameters, commandType: parameterServer.TipoComando);

            parameterServer.Parameters ??= new();
            parameterServer.Parameters!.Add("Result", _result);

            return parameterServer.Parameters;
        }

        public async Task<dynamic?> ExecuteQueryAsync<T>(dynamic parameterServer)
        {
            dynamic? _result = default;

            if (ValidarParametros(parameterServer))
            {
                if (parameterServer.TipoResult == TipoResult.Single) //Single
                {
                    _result = await ExecuteQuerySingleAsync<T>(parameterServer);
                }else if (parameterServer.TipoResult == TipoResult.NoReturn) //Query no Return
                {
                    _result = await ExecuteQueryNoReturnAsync<T>(parameterServer);
                }
                else if (parameterServer.TipoResult == TipoResult.List) //List
                {
                    _result = await ExecuteQueryListAsync<T>(parameterServer);
                }
                else if (parameterServer.TipoResult == TipoResult.Multi) //Multiple
                {
                    _result = await ExecuteQueryMultipleAsync<T>(parameterServer);
                }
                else if (parameterServer.TipoResult == TipoResult.SP) //Execute SP
                {
                    _result = await ExecuteQuerySPAsync<T>(parameterServer);
                }
            }
            else
            {
                throw new ArgumentException("Parámetros incorrectos{0}", JsonConvert.SerializeObject(parameterServer));
            }

            return _result;
        }

        private static bool ValidarParametros<T>(ParameterServer<T> parameterServer)
        {
            bool validar = false;
            string messageError = string.Empty;

            if (string.IsNullOrEmpty(parameterServer.NombreBase))
            {
                messageError = "Debe de contener un nombre de base de datos";
            }
            else if (string.IsNullOrEmpty(parameterServer.NombreServer))
            {
                messageError = "Debe de contener un nombre de servidor de base de datos";
            }
            else if (parameterServer.TipoResult == TipoResult.Multi && parameterServer.Tablas == null)
            {
                messageError = "Debe de asignar una referencia de POCO's";
            }
            else
            {
                validar = true;
            }

            if (!messageError.Equals(""))
            {
                throw new Exception(messageError);
            }

            return validar;
        }     

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    foreach (var _con in _DictCon.Values)
                    {
                        if (_con != null)
                        {
                            if (_con.State != ConnectionState.Closed)
                            {
                                _con.Close();
                                _con.Dispose();
                            }
                        }
                    }

                    foreach (var _con in _DictBD.Values)
                    {
                        if (_con != null)
                        {
                            if (_con.State != ConnectionState.Closed)
                            {
                                _con.Close();
                                _con.Dispose();
                            }
                        }
                    }

                    _DictCon.Clear();
                    _DictBD.Clear();
                }

                disposed = true;
            }
        }
    }
}
