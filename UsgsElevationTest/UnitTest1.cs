﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Usgs.Elevation.Test
{
	/// <summary>
	/// A unit test for testing the elevation client.
	/// </summary>
	[TestClass]
	public class UnitTest1
	{
		/// <summary>
		/// Tests getting an elevation from the USGS elevation service.
		/// </summary>
		[TestMethod]
		public void TestMethod1()
		{
			double x = -122.90167093273578, y = 46.97454264335788;
			var results = ElevationClient.GetElevation(x, y, ElevationUnit.Feet);
			Assert.IsNotNull(results);
			Assert.IsTrue(Math.Abs(x - results.x) < 1, "Difference between input x and output x should be < 1.");
			Assert.IsTrue(Math.Abs(y - results.y) < 1, "Difference between input y and output y should be < 1.");
			Assert.IsTrue(results.Elevation > 0, "Elevation should be greater than zero.");
		}
	}
}
