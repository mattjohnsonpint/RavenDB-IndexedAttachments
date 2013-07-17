Indexed Attachents Bundle for RavenDB
=====================================

This a custom bundle for RavenDB.  The current release requires RavenDB version 2.0.2261 or higher.

It automatically extracts text from attachments as they are uploaded, and indexes the text for searching.

It also adds some convienient metadata for handling attachment filenames and content type.

Full documentation is pending.  Please review the unit tests for example usage.


### Nuget Installation

    PM> Install-Package RavenDB.Bundles.IndexedAttachments

### Manual Installation

Copy the `Raven.Bundles.IndexedAttachments.dll` file to your plugins directory.

Be sure that "IndexedAttachments" is added to your `Raven/ActiveBundles` setting.  Separate other bundle names with semicolons if necessary.

See the unit tests for embedded mode.