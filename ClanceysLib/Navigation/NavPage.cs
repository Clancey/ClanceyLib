using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.Foundation;
namespace ClanceysLib
{
	public class NavPage : UIView
	{
		public List<NavIcon> Icons {get;set;}
		public int Columns = 4;
		public int Rows = 5;
		public float Padding = 10;
		public NavLauncher parent;
		public NavPage():base()
		{
			
		}
		public NavPage (RectangleF rect): base(rect)
		{
			
		}
		
		
		public void Refresh()
		{
			float columnWidth = ((Frame.Width - (Padding * 2 )) / Columns);
			float rowsHeight = ((Frame.Height - (Padding * 2 )) / Rows);
			float currentH = Padding;
			float currentW = 0;	
			foreach(var icon in Icons)
			{
				var x =  (columnWidth - icon.Frame.Width ) /2;
				var y = (rowsHeight - icon.Frame.Height ) /2;
				if(currentW + x >  Frame.Width)
				{
					currentW = 0;
					currentH += rowsHeight + Padding;
				}
				x += currentW ;
				y += currentH ;
				Console.WriteLine(x + " : " + y);
				icon.parent = this;
				icon.Refresh(new PointF( x, y));
				if(icon.Superview != this)
					this.AddSubview(icon);
				currentW += columnWidth;				
			}	
		}
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
		}
		
	}
}

