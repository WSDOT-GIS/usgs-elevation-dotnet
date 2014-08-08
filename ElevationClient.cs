using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Usgs.Ned
{
	/// <summary>
	/// An object that represents the results of a query to the USGS Elevation service.
	/// </summary>
	public class ElevationInfo
	{
		/// <summary>
		/// Longitude
		/// </summary>
		public double x { get; set; }

		/// <summary>
		/// Latitude
		/// </summary>
		public double y { get; set; }

		/// <summary>
		/// The data source of the elevation data.
		/// </summary>
		public string Data_Source { get; set; }

		/// <summary>
		/// Elevation at the given longitude and latitude. The Z coordinate.
		/// </summary>
		public double Elevation { get; set; }

		/// <summary>
		/// Measurement unit of <see cref="ElevationInfo.Elevation"/>: "Feet" or "Inches".
		/// </summary>
		public string Units { get; set; }
	}

	public class ElevationQueryResult
	{
		public ElevationInfo Elevation_Query { get; set; }
	}

	public class ElevationQueryServiceResult
	{
		public ElevationQueryResult USGS_Elevation_Point_Query_Service { get; set; }
	}

	public enum ElevationUnit
	{
		Feet,
		Meters
	}

	/// <summary>
	/// Client for <see href="http://ned.usgs.gov/epqs/pqs.php">USGS NED Point Query Service</see>.
	/// </summary>
	public class ElevationClient
	{
		const string _url = "http://ned.usgs.gov/epqs/pqs.php";

		/// <summary>
		/// Get the elevation for a point.
		/// </summary>
		/// <param name="x">Longitude (WGS84 X coordinate)</param>
		/// <param name="y">Latitude (WGS84 Y coordinate)</param>
		/// <param name="units">Desired elevation measurement unit</param>
		/// <returns>Returns information about the location including the elevation.</returns>
		public async static Task<ElevationInfo> GetElevation(double x, double y, ElevationUnit units)
		{
			Uri uri = new Uri(string.Format("{0}?x={1}&y={2}&units={3}&output=json", _url, x, y, units));

			ElevationQueryServiceResult output = null;

			using (var client = new HttpClient())
			using (var stream = await client.GetStreamAsync(uri))
			using (var textReader = new StreamReader(stream))
			using (var jsonReader = new JsonTextReader(textReader))
			{
				var serializer = JsonSerializer.Create();
				output = serializer.Deserialize<ElevationQueryServiceResult>(jsonReader);
			}

			return output.USGS_Elevation_Point_Query_Service.Elevation_Query;
		}
	}
}
