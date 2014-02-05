using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Raven.Abstractions.Data;
using Raven.Database.Config;
using Raven.Json.Linq;
using Raven.Tests.Helpers;
using Xunit;

namespace Raven.Bundles.IndexedAttachments.Tests
{
    public class ServerModeTests : RavenTestBase
    {
        private const string TestDocPath = @"docs\small.doc";

        protected override void ModifyConfiguration(InMemoryRavenConfiguration configuration)
        {
            // Wire up the bundle to the embedded database
            configuration.Catalog.Catalogs.Add(new AssemblyCatalog(typeof(PutAttachmentTrigger).Assembly));
            configuration.Settings[Constants.ActiveBundles] = "IndexedAttachments";
        }

        [Fact]
        public void Ensure_Index_Is_Built_When_Running_In_Server_Mode()
        {
            using (var documentStore = NewRemoteDocumentStore(runInMemory:false, requestedStorage: "esent"))
            {
                var filename = Path.GetFileName(TestDocPath);
                var key = "articles/1/" + filename;
                using (var stream = new FileStream(TestDocPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var metadata = new RavenJObject();
                    documentStore.DatabaseCommands.PutAttachment(key, null, stream, metadata);
                }

                using (var session = documentStore.OpenSession())
                {
                    var results = session.Advanced.LuceneQuery<object>("Raven/Attachments")
                                         .WhereEquals("Filename", filename)
                                         .WaitForNonStaleResults()
                                         .ToList();

                    Assert.Equal(1, results.Count);

                    dynamic result = results.First();
                    Assert.Equal(filename, result.Filename);
                    Assert.Equal(key, result.AttachmentKey);
                }
            }
        }
    }
}
