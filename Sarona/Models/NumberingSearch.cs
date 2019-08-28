using System.ComponentModel.DataAnnotations;

namespace Sarona.Models
{
    public enum QueryMode
    {
        [Display(Name = "Right Match")]
        RightMatch,
        Exact,
        Like
    }


    public class NpSearch
    {
        public string Prefix { get; set; }
        [Display(Name ="Prefix Query Mode")]
        public QueryMode PrefixMode { get; set; }
        public byte? Min { get; set; }
        public byte? Max { get; set; }
        [Display(Name ="Abbreviation")]
        public string Abb { get; set; }
        [Display(Name = "Subscriber Name")]
        public string SubscriberName { get; set; }
        [Display(Name = "Choose Directions")]
        public LinkDirection[] Directions { get; set; }
        [Display(Name = "Choose Charging Cases")]
        public string[] ChargingCases { get; set; }
        [Display(Name = "Choose Owners")]
        public string[] Owners { get; set; }
        [Display(Name = "Choose Statuses")]
        public NumberingStatus[] Statuses { get; set; }
        [Display(Name = "Choose Rond Types")]
        public GSB[] RondTypes { get; set; }
        [Display(Name = "Choose Number Types")]
        public string[] NumberTypes { get; set; }
        [Display(Name = "Choose Link Types")]
        public LinkType[] LinkTypes { get; set; }
        [Display(Name = "Choose Area Types")]
        public Area[] Areas { get; set; }

    }
}
