using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamUBlobDemo
{
    public class WrapLayout : Layout<View>
    {
        Dictionary<View, SizeRequest> layout_cache = new Dictionary<View, SizeRequest>();

      
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            throw new NotImplementedException();
        }

        public WrapLayout()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
        }

    }
    



}
