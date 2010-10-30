using System;
using System.Drawing;
using ClanceysLib.DayCalendar;
using MonoTouch.UIKit;
using System.Collections.Generic;
namespace ClanceySamples
{
	public class CalendarView : UIView
	{
		public List<CalendarDayEventView> Events;
		public CalendarDaytimelineView Calendar;
		public CalendarView ()
		{
			Events = new List<CalendarDayEventView>()
			{
				new CalendarDayEventView()
				{
					Title = "Event 1",
					Location = "Location 1",
					startDate = DateTime.Now,
					endDate = DateTime.Now.AddHours(1),
				},
				new CalendarDayEventView()
				{
					Title = "Event 2",
					Location = "Location 1",
					startDate = DateTime.Now,
					endDate = DateTime.Now.AddMinutes(30),
				},
				new CalendarDayEventView()
				{
					Title = "Event 3",
					Location = "Location 3",
					startDate = DateTime.Now.AddMinutes(31),
					endDate = DateTime.Now.AddHours(1),
				},
				new CalendarDayEventView()
				{
					Title = "Event 4",
					Location = "Location 4",
					startDate = DateTime.Now.AddHours(2),
					endDate = DateTime.Now.AddHours(3),
				},
			};
			Calendar = new CalendarDaytimelineView()
			{
				Events = Events
			};
			
			this.AddSubview(Calendar);
			
			
		}
		public override void WillMoveToSuperview (UIView newsuper)
		{
			Calendar.Frame = Frame;
		}
	}
}

