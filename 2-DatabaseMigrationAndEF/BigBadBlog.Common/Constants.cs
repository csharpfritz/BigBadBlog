using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBadBlog;

public static class ServiceNames
{

	/// <summary>
	/// Constants for referencing the database containing blog posts
	/// </summary>
	public static class DATABASE_POSTS {

		public const string SERVERNAME = "bigbad-db";
		public const string NAME = "bigbad-database";

	}

	public const string MIGRATION = "database-migration";

	public const string OUTPUTCACHE = "outputcache";

}
