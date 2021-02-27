using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
namespace DiplomaLol
{
    public class GraphPoints
    {
        public GraphPoints()
        {
            Points = new ObservableCollection<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(2, 6),
                new DataPoint(4, 2)
            };

        }
        public IList<DataPoint> Points { get; private set; } 
    }
}
