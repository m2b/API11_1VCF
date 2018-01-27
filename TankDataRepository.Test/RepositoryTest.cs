using System;
using Xunit;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;

namespace APIVCF
{
    public class RepositoryTest:IClassFixture<FixtureSetup>
    {
        static FixtureSetup _fixture;

        public RepositoryTest(FixtureSetup fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestConstructor()
        {
            RdbmsTankDataRepository<SqlConnection> repos = new RdbmsTankDataRepository<SqlConnection>(_fixture.ExeDirectory);
            Assert.NotNull(repos);
        }
    }

    public class FixtureSetup:IDisposable
    {
        public string ExeDirectory { get; set; }

        public FixtureSetup()
		{
			ExeDirectory = Path.GetDirectoryName(new Uri(typeof(RepositoryTest).GetTypeInfo().Assembly.CodeBase).LocalPath);
		}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FixtureSetup() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
