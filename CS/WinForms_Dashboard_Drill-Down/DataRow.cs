using System;
using System.Collections.Generic;
using System.Linq;

namespace WinForms_Dashboard_Drill_Down {
    public class DataRow {
        public static List<DataRow> GetData() {
            List<DataRow> data = new List<DataRow>();
            for(int i = 1; i < 10; i++)
                data.Add(new DataRow() { Dimension1 = "A" + i, Dimension2 = "AA" + i, Dimension3 = "AAA" + i, Measure = i });
            return data;
        }

        public string Dimension1 { get; set; }
        public string Dimension2 { get; set; }
        public string Dimension3 { get; set; }
        public int Measure { get; set; }
    }
}
