using System;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Usgs.Elevation
{
	/// <summary>
	/// An object that represents the results of a query to the USGS Elevation service.
	/// </summary>
	[DataContract]
	public class ElevationInfo
	{
		[DataMember(IsRequired = true)]
		public double x { get; set; }

		[DataMember(IsRequired = true)]
		public double y { get; set; }

		[DataMember(IsRequired = true)]
		public string Data_Source { get; set; }

		[DataMember(IsRequired = true)]
		public double Elevation { get; set; }

		/// <summary>
		/// Measurement unit of elevation: "Feet" or "Inches".
		/// </summary>
		[DataMember(IsRequired = true)]
		public string Units { get; set; }
	}

	public class ElevationQueryResult
	{
		[DataMember(IsRequired = true)]
		public ElevationInfo Elevation_Query { get; set; }
	}

	[DataContract]
	public class ElevationQueryServiceResult
	{
		[DataMember(IsRequired = true)]
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
		public static ElevationInfo GetElevation(double x, double y, ElevationUnit units)
		{
			Uri uri = new Uri(string.Format("{0}?x={1}&y={2}&units={3}&output=json", _url, x, y, units));

			ElevationQueryServiceResult output = null;

			var request = HttpWebRequest.CreateDefault(uri);


			using (var response = request.GetResponse() as HttpWebResponse)
			using (var stream = response.GetResponseStream())
			{
				var serializer = new DataContractJsonSerializer(typeof(ElevationQueryServiceResult));
				output = (ElevationQueryServiceResult)serializer.ReadObject(stream);
			}


			return output.USGS_Elevation_Point_Query_Service.Elevation_Query;

		}
	}
}
