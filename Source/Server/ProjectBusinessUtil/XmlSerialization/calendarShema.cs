﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 
namespace ProjectBusinessUtil.XmlSerialization {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("Calendar", Namespace = "http://schemas.microsoft.com/project", IsNullable = false)]
    public partial class CalendarType {
        
        private string uIDField;
        
        private string nameField;
        
        private bool isBaseCalendarField;
        
        private bool isBaseCalendarFieldSpecified;
        
        private string baseCalendarUIDField;
        
        private CalendarWeekDay[] weekDaysField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string UID {
            get {
                return this.uIDField;
            }
            set {
                this.uIDField = value;
            }
        }
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public bool IsBaseCalendar {
            get {
                return this.isBaseCalendarField;
            }
            set {
                this.isBaseCalendarField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsBaseCalendarSpecified {
            get {
                return this.isBaseCalendarFieldSpecified;
            }
            set {
                this.isBaseCalendarFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string BaseCalendarUID {
            get {
                return this.baseCalendarUIDField;
            }
            set {
                this.baseCalendarUIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("WeekDay", IsNullable=false)]
        public CalendarWeekDay[] WeekDays {
            get {
                return this.weekDaysField;
            }
            set {
                this.weekDaysField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/project")]
    [System.Xml.Serialization.XmlRootAttribute("WeekDay", Namespace = "http://schemas.microsoft.com/project", IsNullable = false)]
    public partial class CalendarWeekDay {
        
        private CalendarWeekDayDayType dayTypeField;
        
        private bool dayWorkingField;
        
        private bool dayWorkingFieldSpecified;
        
        private CalendarWeekDayTimePeriod timePeriodField;
        
        private CalendarWeekDayWorkingTimes workingTimesField;
        
        /// <remarks/>
        public CalendarWeekDayDayType DayType {
            get {
                return this.dayTypeField;
            }
            set {
                this.dayTypeField = value;
            }
        }
        
        /// <remarks/>
        public bool DayWorking {
            get {
                return this.dayWorkingField;
            }
            set {
                this.dayWorkingField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DayWorkingSpecified {
            get {
                return this.dayWorkingFieldSpecified;
            }
            set {
                this.dayWorkingFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public CalendarWeekDayTimePeriod TimePeriod {
            get {
                return this.timePeriodField;
            }
            set {
                this.timePeriodField = value;
            }
        }
        
        /// <remarks/>
        public CalendarWeekDayWorkingTimes WorkingTimes {
            get {
                return this.workingTimesField;
            }
            set {
                this.workingTimesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/project")]
    public enum CalendarWeekDayDayType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Exception,
 
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Sunday,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Monday,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Tuesday,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Wednesday,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Thursday,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Friday,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Saturday,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/project")]
    [System.Xml.Serialization.XmlRootAttribute("TimePeriod", Namespace = "http://schemas.microsoft.com/project", IsNullable = false)]
    public partial class CalendarWeekDayTimePeriod {
        
        private System.DateTime fromDateField;
        
        private bool fromDateFieldSpecified;
        
        private System.DateTime toDateField;
        
        private bool toDateFieldSpecified;
        
        /// <remarks/>
        public System.DateTime FromDate {
            get {
                return this.fromDateField;
            }
            set {
                this.fromDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FromDateSpecified {
            get {
                return this.fromDateFieldSpecified;
            }
            set {
                this.fromDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ToDate {
            get {
                return this.toDateField;
            }
            set {
                this.toDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ToDateSpecified {
            get {
                return this.toDateFieldSpecified;
            }
            set {
                this.toDateFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/project")]
    [System.Xml.Serialization.XmlRootAttribute("WorkingTimes", Namespace = "http://schemas.microsoft.com/project", IsNullable = false)]
    public partial class CalendarWeekDayWorkingTimes {
        
        private CalendarWeekDayWorkingTimesWorkingTime[] itemsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WorkingTime")]
        public CalendarWeekDayWorkingTimesWorkingTime[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute("WorkingTime", Namespace = "http://schemas.microsoft.com/project", IsNullable = false)]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/project")]
    public partial class CalendarWeekDayWorkingTimesWorkingTime {
        
        private System.DateTime fromTimeField;
        
        private bool fromTimeFieldSpecified;
        
        private System.DateTime toTimeField;
        
        private bool toTimeFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="time")]
        public System.DateTime FromTime {
            get {
                return this.fromTimeField;
            }
            set {
                this.fromTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FromTimeSpecified {
            get {
                return this.fromTimeFieldSpecified;
            }
            set {
                this.fromTimeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="time")]
        public System.DateTime ToTime {
            get {
                return this.toTimeField;
            }
            set {
                this.toTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ToTimeSpecified {
            get {
                return this.toTimeFieldSpecified;
            }
            set {
                this.toTimeFieldSpecified = value;
            }
        }
    }
}
