USGS Elevation service client
=============================

This is a .NET library for calling the [USGS National Elevation Dataset Point Query Service].

[USGS National Elevation Dataset Point Query Service]:http://ned.usgs.gov/epqs/

## Example ##

```c#
double x = -122.90167093273578, y = 46.97454264335788;
Usgs.Ned.ElevationInfo results = null;
Usgs.Ned.ElevationClient.GetElevation(x, y, ElevationUnit.Feet).ContinueWith(t =>
{
results = t.Result;
}).Wait();
```