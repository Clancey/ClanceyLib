using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.CoreAnimation;
using System.Linq;
namespace ClanceysLib
{
	namespace DayCalendar
	{

		internal class Block
		{
			internal ArrayList Columns;
			public List<CalendarDayEventView> events = new List<CalendarDayEventView> ();


			internal Block ()
			{
			}

			internal void Add (CalendarDayEventView ev)
			{
				events.Add (ev);
				arrangeColumns ();
			}

			private BlockColumn createColumn ()
			{
				BlockColumn col = new BlockColumn ();
				this.Columns.Add (col);
				col.Block = this;
				
				return col;
			}
			private void arrangeColumns ()
			{
				// cleanup
				this.Columns = new ArrayList ();
				
				foreach (CalendarDayEventView e in events)
					e.Column = null;
				
				// there always will be at least one column because arrangeColumns is called only from Add()
				createColumn ();
				
				foreach (CalendarDayEventView e in events)
				{
					foreach (BlockColumn col in Columns)
					{
						if (col.CanAdd (e))
						{
							col.Add (e);
							break;
						}
					}
					// it wasn't placed 
					if (e.Column == null)
					{
						BlockColumn col = createColumn ();
						col.Add (e);
					}
				}
			}


			internal bool OverlapsWith (CalendarDayEventView e)
			{
				if (events.Count == 0)
					return false;
				
				return (this.BoxStart < e.BoxEnd && this.BoxEnd > e.startDate);
			}

			internal DateTime BoxStart {

				get { return (from e in events.ToArray ()select e.BoxStart).Min (); }
			}

			internal DateTime BoxEnd {
				get { return (from e in events.ToArray ()select e.BoxEnd).Max (); }
			}
			
		}

		internal class BlockColumn
		{
			private ArrayList events = new ArrayList ();
			internal Block Block;


			private bool isLastInBlock {
				get { return Block.Columns[Block.Columns.Count - 1] == this; }
			}

			internal BlockColumn ()
			{
			}

			internal bool CanAdd (CalendarDayEventView e)
			{
				foreach (CalendarDayEventView ev in events)
				{
					if (ev.OverlapsWith (e))
						return false;
				}
				return true;
			}

			internal void Add (CalendarDayEventView e)
			{
				if (e.Column != null)
					throw new ApplicationException ("This Event was already placed into a Column.");
				
				events.Add (e);
				e.Column = this;
			}

			/// <summary>
			/// Gets the order number of the column.
			/// </summary>
			public int Number {
				get {
					if (Block == null)
						throw new ApplicationException ("This Column doesn't belong to any Block.");
					
					return Block.Columns.IndexOf (this);
				}
			}
		}

		public class CalendarDayEventView : UIView
		{
			private static float HORIZONTAL_OFFSET = 4.0f;
			private static float VERTICAL_OFFSET = 5.0f;
			private static float VERTICAL_DIFF = 50.0f;

			private static float FONT_SIZE = 12.0f;

			public int id { get; set; }
			public DateTime startDate { get; set; }
			public DateTime endDate { get; set; }
			public string Title { get; set; }
			public string location { get; set; }
			internal BlockColumn Column { get; set; }
			public CalendarDayEventView ()
			{
				this.Frame = new RectangleF (0, 0, 320, 400);
				setupCustomInitialisation ();
			}
			public CalendarDayEventView (RectangleF frame)
			{
				this.Frame = frame;
				setupCustomInitialisation ();
			}

			public DateTime BoxStart {
				get {
					if (startDate.Minute >= 30)
						return new DateTime (startDate.Year, startDate.Month, startDate.Day, startDate.Hour, 30, 0);
					else
						return new DateTime (startDate.Year, startDate.Month, startDate.Day, startDate.Hour, 0, 0);
				}
			}
			public bool OverlapsWith (CalendarDayEventView e)
			{
				return (this.BoxStart < e.BoxEnd && this.BoxEnd > e.startDate);
			}

			public DateTime BoxEnd {
				get {
					if (endDate.Minute > 30)
					{
						DateTime hourPlus = endDate.AddHours (1);
						return new DateTime (hourPlus.Year, hourPlus.Month, hourPlus.Day, hourPlus.Hour, 0, 0);
					}


					else if (endDate.Minute > 0)
					{
						return new DateTime (endDate.Year, endDate.Month, endDate.Day, endDate.Hour, 30, 0);
					}
					else
					{
						return new DateTime (endDate.Year, endDate.Month, endDate.Day, endDate.Hour, 0, 0);
					}
				}
			}


			public void setupCustomInitialisation ()
			{
				
				
				this.BackgroundColor = UIColor.Purple;
				this.Alpha = 0.8f;
				CALayer layer = this.Layer;
				layer.MasksToBounds = true;
				layer.CornerRadius = 5.0f;
				// You can even add a border
				layer.BorderWidth = 0.5f;
				layer.BorderColor = UIColor.LightGray.CGColor;
			}

			public override void Draw (RectangleF rect)
			{
				// Retrieve the graphics context 
				var context = UIGraphics.GetCurrentContext ();
				//CGContextRef context = new UIGraphicsGetCurrentContext();
				
				// Save the context state 
				context.SaveState ();
				
				
				// Set shadow
				context.SetShadowWithColor (new SizeF (0.0f, 1.0f), 0.7f, UIColor.Black.CGColor);
				
				// Set text color
				UIColor.White.SetColor ();
				
				RectangleF titleRect = new RectangleF (this.Bounds.X + HORIZONTAL_OFFSET, this.Bounds.Y + VERTICAL_OFFSET, this.Bounds.Width - 2 * HORIZONTAL_OFFSET, FONT_SIZE + 4.0f);
				
				RectangleF locationRect = new RectangleF (this.Bounds.X + HORIZONTAL_OFFSET, this.Bounds.Y + VERTICAL_OFFSET + FONT_SIZE + 4.0f, this.Bounds.Width - 2 * HORIZONTAL_OFFSET, FONT_SIZE + 4.0f);
				
				// Drawing code
				if (this.Bounds.Height > VERTICAL_DIFF)
				{
					// Draw both title and location
					if (!string.IsNullOrEmpty (this.Title))
					{
						DrawString (Title, titleRect, UIFont.BoldSystemFontOfSize (FONT_SIZE), UILineBreakMode.TailTruncation, UITextAlignment.Left);
						
					}
					if (!string.IsNullOrEmpty (location))
					{
						DrawString (location, locationRect, UIFont.SystemFontOfSize (FONT_SIZE), UILineBreakMode.TailTruncation, UITextAlignment.Left);
						
						
					}
				}

				
				else
				{
					// Draw only title
					if (!string.IsNullOrEmpty (Title))
					{
						DrawString (Title, titleRect, UIFont.BoldSystemFontOfSize (FONT_SIZE), UILineBreakMode.TailTruncation, UITextAlignment.Left);
						
					}
				}
				
				// Restore the context state
				context.RestoreState ();
			}
			
		}


		//public delegate void 
		public class CalendarDaytimelineView : UIView
		{
			//////

			public static float HORIZONTAL_OFFSET = 3.0f;
			public static float VERTICAL_OFFSET = 5.0f;
			public static float TIME_WIDTH = 20.0f;
			public static float PERIOD_WIDTH = 26.0f;

			private static float VERTICAL_DIFF = 50.0f;
			public static float FONT_SIZE = 14.0f;

			public static float HORIZONTAL_LINE_DIFF = 10.0f;

			public static float TIMELINE_HEIGHT = (24 * VERTICAL_OFFSET) + (23 * VERTICAL_DIFF);

			public static float EVENT_VERTICAL_DIFF = 0.0f;
			public static float EVENT_HORIZONTAL_DIFF = 2.0f;
			public List<CalendarDayEventView> events;
			public DateTime currentDate;
			private UIScrollView scrollView;
			private TimeLineView timelineView;



			#pragma mark -
			#pragma mark Initialisation

// The designated initializer. Override to perform setup that is required before the view is loaded.
// Only when xibless (interface buildder)

			public CalendarDaytimelineView (RectangleF rect)
			{
				this.Frame = rect;
				setupCustomInitialisation ();
			}
			public CalendarDaytimelineView ()
			{
				
				this.Frame = new RectangleF (0, 0, 320, 400);
				setupCustomInitialisation ();
			}


			public void setupCustomInitialisation ()
			{
				// Initialization code
				events = new List<CalendarDayEventView> ();
				currentDate = DateTime.Today;
				// Add main scroll view
				this.AddSubview (getScrollView ());
				// Add timeline view inside scrollview
				scrollView.AddSubview (getTimeLineView ());
				
			}

			private UIScrollView getScrollView ()
			{
				if (scrollView == null)
				{
					scrollView = new UIScrollView (this.Bounds);
					scrollView.ContentSize = new SizeF (this.Bounds.Size.Width, TIMELINE_HEIGHT);
					scrollView.ScrollEnabled = true;
					scrollView.BackgroundColor = UIColor.White;
					scrollView.AlwaysBounceVertical = true;
					
				}
				return scrollView;
				
			}


			#pragma mark -
			#pragma mark Setup



			private TimeLineView getTimeLineView ()
			{
				
				if (timelineView == null)
				{
					timelineView = new TimeLineView (new RectangleF (this.Bounds.X, this.Bounds.Y, this.Bounds.Size.Width, TIMELINE_HEIGHT));
					timelineView.BackgroundColor = UIColor.White;
					
					
				}
				
				return timelineView;
			}

			#pragma mark -
			#pragma mark View Event
			public override void MovedToWindow ()
			{
				
				if (Window != null)
					this.reloadDay ();
				
			}


			#pragma mark -
			#pragma mark Reload Day

			private void reloadDay ()
			{
				// If no current day was given
				// Make it today
				if (currentDate != null)
				{
					// Dont' want to inform the observer
					currentDate = DateTime.Today;
				}
				
				// Remove all previous view event
				foreach (var view in this.scrollView.Subviews)
				{
					if (view is TimeLineView)
					{
					}

					
					else
					{
						view.RemoveFromSuperview ();
					}
				}
				
				// Ask the delgate about the events that correspond
				// the the currently displayed day view
				if (events != null)
				{
					
					events = events.OrderBy (x => x.startDate).ThenByDescending (x => x.endDate).ToList ();
					
					List<Block> blocks = new List<Block> ();
					Block lastBlock = new Block ();
					foreach (CalendarDayEventView e in events)
					{
						// if there is no block, create the first one
						if (blocks.Count == 0)
						{
							lastBlock = new Block ();
							blocks.Add (lastBlock);
						}
						// or if the event doesn't overlap with the last block, create a new block


						else if (!lastBlock.OverlapsWith (e))
						{
							lastBlock = new Block ();
							blocks.Add (new Block ());
						}
						
						// any case, add it to some block
						lastBlock.Add (e);
						
					}
					foreach (Block theBlock in blocks)
					{
						foreach (CalendarDayEventView theEvent in theBlock.events)
						{
							
							
							// Making sure delgate sending date that match current day
							if (theEvent.startDate.Date == currentDate)
							{
								// Get the hour start position
								Int32 hourStart = theEvent.startDate.Hour;
								float hourStartPosition = (float)Math.Round ((hourStart * VERTICAL_DIFF) + VERTICAL_OFFSET + ((FONT_SIZE + 4.0f) / 2.0f));
								// Get the minute start position
								// Round minute to each 5
								Int32 minuteStart = theEvent.startDate.Minute;
								minuteStart = Convert.ToInt32 (Math.Round (minuteStart / 5.0f) * 5);
								float minuteStartPosition = (float)Math.Round ((minuteStart < 30) ? 0 : VERTICAL_DIFF / 2.0);
								
								
								
								// Get the hour end position
								Int32 hourEnd = theEvent.endDate.Hour;
								if (theEvent.startDate.Date != theEvent.endDate.Date)
								{
									hourEnd = 23;
								}
								float hourEndPosition = (float)Math.Round ((hourEnd * VERTICAL_DIFF) + VERTICAL_OFFSET + ((FONT_SIZE + 4.0f) / 2.0f));
								// Get the minute end position
								// Round minute to each 5
								Int32 minuteEnd = theEvent.endDate.Minute;
								if (theEvent.startDate.Date != theEvent.endDate.Date)
								{
									minuteEnd = 55;
								}
								minuteEnd = Convert.ToInt32 (Math.Round (minuteEnd / 5.0) * 5);
								float minuteEndPosition = (float)Math.Round ((minuteEnd < 30) ? 0 : VERTICAL_DIFF / 2.0f);
								
								float eventHeight = 0.0f;
								
								if (minuteStartPosition == minuteEndPosition || hourEnd == 23)
								{
									// Starting and ending date position are the same
									// Take all half hour space
									// Or hour is at the end
									eventHeight = (VERTICAL_DIFF / 2) - (2 * EVENT_VERTICAL_DIFF);
								}

								
								else
								{
									// Take all hour space
									eventHeight = VERTICAL_DIFF - (2 * EVENT_VERTICAL_DIFF);
								}
								
								if (hourStartPosition != hourEndPosition)
								{
									eventHeight += (hourEndPosition + minuteEndPosition) - hourStartPosition - minuteStartPosition;
								}
								
								var availableWidth = this.Bounds.Size.Width - (HORIZONTAL_OFFSET + TIME_WIDTH + PERIOD_WIDTH + HORIZONTAL_LINE_DIFF) - HORIZONTAL_LINE_DIFF - EVENT_HORIZONTAL_DIFF;
								var currentWidth = availableWidth / theBlock.Columns.Count;
								var currentInt = theEvent.Column.Number;
								var x = HORIZONTAL_OFFSET + TIME_WIDTH + PERIOD_WIDTH + HORIZONTAL_LINE_DIFF + EVENT_HORIZONTAL_DIFF + (currentWidth * currentInt);
								var y = hourStartPosition + minuteStartPosition + EVENT_VERTICAL_DIFF;
								RectangleF eventFrame = new RectangleF (x, y, currentWidth, eventHeight);
								
								theEvent.Frame = eventFrame;
								//event.delegate = self;
								theEvent.SetNeedsDisplay ();
								this.scrollView.AddSubview (theEvent);
								
								
								// Log the extracted date values
								Console.WriteLine ("hourStart: {0} minuteStart: {1}", hourStart, minuteStart);
							}
						}
					}
				}
			}

			public class TimeLineView : UIView
			{




				string[] _times;
				string[] _periods;
// The designated initializer. Override to perform setup that is required before the view is loaded.
// Only when xibless (interface buildder)
				public TimeLineView (RectangleF rect)
				{
					this.Frame = rect;
					setupCustomInitialisation ();
				}


				public void setupCustomInitialisation ()
				{
					// Initialization code
					
				}

				#pragma mark -
				#pragma mark Setup

// Setup array consisting of string
// representing time aka 12 (12 am), 1 (1 am) ... 25 x

				public string[] times {
					get {
						if (_times == null)
						{
							_times = new string[] { "12", "1", "2", "3", "4", "5", "6", "7", "8", "9",
							"10", "11", "Noon", "1", "2", "3", "4", "5", "6", "7",
							"8", "9", "10", "11", "12", "" };
							
						}
						return _times;
					}
				}

// Setup array consisting of string
// representing time periods aka AM or PM
// Matching the array of times 25 x

				public string[] periods {
					get {
						
						if (_periods == null)
						{
							_periods = new string[] { "AM", "AM", "AM", "AM", "AM", "AM", "AM", "AM", "AM", "AM",
							"AM", "AM", "", "PM", "PM", "PM", "PM", "PM", "PM", "PM",
							"PM", "PM", "PM", "PM", "AM", "" };
						}
						return _periods;
					}
				}

				#pragma mark -
				#pragma mark Drawing

				public override void Draw (RectangleF rect)
				{
					// Drawing code
					// Here Draw timeline from 12 am to noon to 12 am next day
					// Times appearance
					
					UIFont timeFont = UIFont.BoldSystemFontOfSize (FONT_SIZE);
					UIColor timeColor = UIColor.Black;
					
					// Periods appearance
					UIFont periodFont = UIFont.SystemFontOfSize (FONT_SIZE);
					UIColor periodColor = UIColor.Gray;
					
					// Draw each times string
					for (Int32 i = 0; i < this.times.Length; i++)
					{
						// Draw time
						timeColor.SetStroke ();
						string time = this.times[i];
						
						RectangleF timeRect = new RectangleF (HORIZONTAL_OFFSET, VERTICAL_OFFSET + i * VERTICAL_DIFF, TIME_WIDTH, FONT_SIZE + 4.0f);
						
						// Find noon
						if (i == 24 / 2)
						{
							timeRect = new RectangleF (HORIZONTAL_OFFSET, VERTICAL_OFFSET + i * VERTICAL_DIFF, TIME_WIDTH + PERIOD_WIDTH, FONT_SIZE + 4.0f);
						}
						
						DrawString (time, timeRect, timeFont, UILineBreakMode.WordWrap, UITextAlignment.Right);
						
						
						// Draw period
						// Only if it is not noon
						if (i != 24 / 2)
						{
							periodColor.SetStroke ();
							
							string period = this.periods[i];
							DrawString (period, new RectangleF (HORIZONTAL_OFFSET + TIME_WIDTH, VERTICAL_OFFSET + i * VERTICAL_DIFF, PERIOD_WIDTH, FONT_SIZE + 4.0f), periodFont, UILineBreakMode.WordWrap, UITextAlignment.Right);
							
							
							var context = UIGraphics.GetCurrentContext ();
							
							// Save the context state 
							context.SaveState ();
							context.SetStrokeColorWithColor (UIColor.LightGray.CGColor);
							
							// Draw line with a black stroke color
							// Draw line with a 1.0 stroke width
							context.SetLineWidth (0.5f);
							// Translate context for clear line
							context.TranslateCTM (-0.5f, -0.5f);
							context.BeginPath ();
							context.MoveTo (HORIZONTAL_OFFSET + TIME_WIDTH + PERIOD_WIDTH + HORIZONTAL_LINE_DIFF, VERTICAL_OFFSET + i * VERTICAL_DIFF + (float)Math.Round (((FONT_SIZE + 4.0) / 2.0)));
							context.AddLineToPoint (this.Bounds.Size.Width, VERTICAL_OFFSET + i * VERTICAL_DIFF + (float)Math.Round ((FONT_SIZE + 4.0) / 2.0));
							context.StrokePath ();
							
							if (i != this.times.Length - 1)
							{
								context.BeginPath ();
								context.MoveTo (HORIZONTAL_OFFSET + TIME_WIDTH + PERIOD_WIDTH + HORIZONTAL_LINE_DIFF, VERTICAL_OFFSET + i * VERTICAL_DIFF + (float)Math.Round (((FONT_SIZE + 4.0f) / 2.0f)) + (float)Math.Round ((VERTICAL_DIFF / 2.0f)));
								context.AddLineToPoint (this.Bounds.Size.Width, VERTICAL_OFFSET + i * VERTICAL_DIFF + (float)Math.Round (((FONT_SIZE + 4.0f) / 2.0f)) + (float)Math.Round ((VERTICAL_DIFF / 2.0f)));
								float[] dash1 = { 2.0f, 1.0f };
								context.SetLineDash (0.0f, dash1, 2);
								context.StrokePath ();
							}
							
							// Restore the context state
							context.RestoreState ();
							
							
							
						}
					}
					
					
				}
				
				///////
			}
			
		}
		
	}
}

