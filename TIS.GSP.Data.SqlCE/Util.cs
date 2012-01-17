using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Data.SqlCe
{
	#region Enum Declarations

	/// <summary>
	/// References the name of a SQL upgrade script embedded in this assembly that can be used to upgrade the 
	/// database schema.
	/// </summary>
	internal enum GalleryDataSchemaUpgradeScript
	{
		///// <summary>
		///// Gets the script file that upgrades the database from 2.1.3162 to 2.3.3421.
		///// </summary>
		//SqlUpgrade_2_1_3162_to_2_3_3421 = 0,
		///// <summary>
		///// Gets the script file that upgrades the database from 2.3.3421 to 2.4.6.
		///// </summary>
		//SqlUpgrade_2_3_3421_to_2_4_6 = 1,
		///// <summary>
		///// Gets the script file that upgrades the database from 2.4.1 to 2.4.3.
		///// </summary>
		//SqlUpgrade_2_4_1_to_2_4_3 = 2,
		///// <summary>
		///// Gets the script file that upgrades the database from 2.4.3 to 2.4.4.
		///// </summary>
		//SqlUpgrade_2_4_3_to_2_4_4 = 3,
		///// <summary>
		///// Gets the script file that upgrades the database from 2.4.4 to 2.4.5.
		///// </summary>
		//SqlUpgrade_2_4_4_to_2_4_5 = 4,
		///// <summary>
		///// Gets the script file that upgrades the database from 2.4.5 to 2.4.6.
		///// </summary>
		//SqlUpgrade_2_4_5_to_2_4_6 = 5
	}

	#endregion

	/// <summary>
	/// Contains functionality for commonly used functions used throughout this assembly.
	/// </summary>
	/// <remarks>This is the same class as the identically named one in the SqlServer project. Any changes made to this class
	/// should be made to that one as well.</remarks>
	internal static class Util
	{
		#region Fields

		internal const GalleryDataSchemaVersion RequiredDatabaseSchemaVersion = GalleryDataSchemaVersion.V2_5_0;
		private const string ExceptionSqlId = "SQL";

		private static string _connectionString;
		private static string _connectionStringName;

		#endregion

		#region Properties

		public static string ConnectionString
		{
			get
			{
				return _connectionString;
			}
			set
			{
				_connectionString = value;
			}
		}

		public static string ConnectionStringName
		{
			get
			{
				return _connectionStringName;
			}
			set
			{
				_connectionStringName = value;
			}
		}

		#endregion

		#region Internal Methods

		/// <overloads>
		/// Get a reference to the database connection used for gallery data.
		/// </overloads>
		/// <summary>
		/// Get a reference to a new, closed database connection used for gallery data.
		/// </summary>
		/// <returns>A <see cref="SqlCeConnection"/> instance.</returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		internal static SqlCeConnection GetDbConnectionForGallery()
		{
			return GetDbConnectionForGallery(null);
		}

		/// <summary>
		/// Get a reference to a new, closed database connection using the specified <paramref name="connectionString" />.
		/// </summary>
		/// <param name="connectionString">The connection string to use. This parameter can be set to null or <see cref="String.Empty" />
		/// when calling this function from an initialized data provider, since in that case it will use the connection string passed to the 
		/// data provider.</param>
		/// <returns>A <see cref="SqlCeConnection"/> instance.</returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		internal static SqlCeConnection GetDbConnectionForGallery(string connectionString)
		{
			if (String.IsNullOrEmpty(connectionString))
			{
				return new SqlCeConnection(ConnectionString);
			}
			else
			{
				return new SqlCeConnection(connectionString);
			}
		}

		/// <summary>
		/// Validates that the backup file specified in the <see cref="IBackupFile.FilePath"/> property of the <paramref name="backupFile"/>
		/// parameter is valid and populates the remaining properties with information about the file.
		/// </summary>
		/// <param name="backupFile">An instance of <see cref="IBackupFile"/> that with only the <see cref="IBackupFile.FilePath"/>
		/// property assigned. The remaining properties should be uninitialized since they will be assigned in this method.</param>
		/// <remarks>Note that this function attempts to extract the number of records from each table in the backup file. Any exceptions
		/// that occur during this process are caught and trigger the <see cref="IBackupFile.IsValid" /> property to be set to false. If the extraction is 
		/// successful, then the file is assumed to be valid and the <see cref="IBackupFile.IsValid" /> property is set to <c>true</c>.</remarks>
		internal static void ValidateBackupFile(ref IBackupFile backupFile)
		{
			try
			{
				using (DataSet ds = GenerateDataSet(backupFile.FilePath))
				{
					string[] tableNames = new string[] { "aspnet_Applications", "aspnet_Profile", "aspnet_Roles", "aspnet_Membership", "aspnet_Users", "aspnet_UsersInRoles", 
					                                     "gs_Gallery", "gs_Album", "gs_MediaObject", "gs_MediaObjectMetadata", "gs_Role_Album", "gs_Role", 
					                                     "gs_AppError", "gs_AppSetting", "gs_GalleryControlSetting", "gs_GallerySetting", "gs_BrowserTemplate", 
					                                     "gs_MimeType", "gs_MimeTypeGallery", "gs_UserGalleryProfile" };

					foreach (string tableName in tableNames)
					{
						DataTable table = ds.Tables[tableName];

						backupFile.DataTableRecordCount.Add(tableName, table.Rows.Count);
					}

					backupFile.SchemaVersion = GetDataSchemaVersionString();

					if (backupFile.SchemaVersion == GalleryDataSchemaVersionEnumHelper.ConvertGalleryDataSchemaVersionToString(GalleryDataSchemaVersion.V2_5_0))
					{
						backupFile.IsValid = true;
					}
				}
			}
			catch
			{
				backupFile.IsValid = false;
			}
		}

		/// <summary>
		/// Executes the SQL script represented by the specified <paramref name="script"/>. The actual SQL file is stored as an
		/// embedded resource in the current assembly.
		/// </summary>
		/// <param name="script">The script to execute. This value is used to lookup the SQL file stored as an embedded resource
		/// in the current assembly.</param>
		internal static void ExecuteSqlUpgradeScript(GalleryDataSchemaUpgradeScript script)
		{
			ExecuteSqlUpgradeScript(script, null);
		}

		/// <summary>
		/// Executes the SQL script represented by the specified <paramref name="script"/>. The actual SQL file is stored as an
		/// embedded resource in the current assembly.
		/// </summary>
		/// <param name="script">The script to execute. This value is used to lookup the SQL file stored as an embedded resource
		/// in the current assembly.</param>
		/// <param name="connectionString">The connection string to use. This parameter can be set to null or <see cref="String.Empty" />
		/// when calling this function from an initialized data provider, since in that case it will use the connection string passed to the 
		/// data provider.</param>
		internal static void ExecuteSqlUpgradeScript(GalleryDataSchemaUpgradeScript script, string connectionString)
		{
			System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
			string scriptLocation = String.Format(CultureInfo.InvariantCulture, "GalleryServerPro.Data.SqlCe.{0}.sql", script);
			using (Stream stream = asm.GetManifestResourceStream(scriptLocation))
			{
				if (stream == null)
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Unable to find embedded resource file named {0}", scriptLocation));

				ExecuteSqlInStream(stream, connectionString);
			}
		}

		/// <summary>
		/// Execute the SQL statements in the specified stream.
		/// </summary>
		/// <param name="stream">A stream containing a series of SQL statements separated by the word GO.</param>
		/// <param name="connectionString">The connection string to use. This parameter can be set to null or <see cref="String.Empty" />
		/// when calling this function from an initialized data provider, since in that case it will use the connection string passed to the 
		/// data provider.</param>
		/// <remarks>This function is copied from the install wizard in the web project.</remarks>
		private static void ExecuteSqlInStream(Stream stream, string connectionString)
		{
			const int timeout = 600; // Timeout for SQL Execution (seconds)
			StreamReader sr = null;
			StringBuilder sb = new StringBuilder();
			SqlCeTransaction tran = null;

			try
			{
				sr = new StreamReader(stream);
				using (SqlCeConnection cn = GetDbConnectionForGallery(connectionString))
				{
					cn.Open();
					tran = cn.BeginTransaction();

					while (!sr.EndOfStream)
					{
						if (sb.Length > 0) sb.Remove(0, sb.Length); // Clear out string builder

						using (SqlCeCommand cmd = cn.CreateCommand())
						{
							while (!sr.EndOfStream)
							{
								string s = sr.ReadLine();
								if (s != null && s.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
								{
									break;
								}

								sb.AppendLine(s);
							}

							// Execute T-SQL against the target database
							cmd.CommandText = sb.ToString();
							cmd.CommandTimeout = timeout;

							cmd.ExecuteNonQuery();
						}
					}

					tran.Commit();
				}
			}
			catch (Exception ex)
			{
				if (tran != null)
				{
					try
					{
						tran.Rollback();
					}
					catch (Exception) { }
				}

				if (!ex.Data.Contains(ExceptionSqlId))
				{
					ex.Data.Add(ExceptionSqlId, sb.ToString());
				}
				throw;
			}
			finally
			{
				if (sr != null)
					sr.Close();
			}
		}

		#endregion

		#region Private Functions

		private static DataSet GenerateDataSet(string filePath)
		{
			DataSet ds = null;
			try
			{
				ds = new DataSet("GalleryServerData");
				ds.Locale = CultureInfo.InvariantCulture;
				ds.ReadXml(filePath, XmlReadMode.Auto);
			}
			catch
			{
				if (ds != null)
					ds.Dispose();

				throw;
			}

			return ds;
		}

		#endregion

		/// <summary>
		/// Gets the data schema version of the database. May return null. Examples: "2.3.3421", "2.4.1"
		/// </summary>
		/// <returns>Returns a <see cref="string"/> containing the database version.</returns>
		internal static string GetDataSchemaVersionString()
		{
			using (GspContext ctx = new GspContext())
			{
				return (from a in ctx.AppSettings where a.SettingName == "DataSchemaVersion" select a.SettingValue).FirstOrDefault();
			}
		}
	}
}
