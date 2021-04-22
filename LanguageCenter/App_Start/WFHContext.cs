namespace LanguageCenter.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Linq; 

    public class WFHContext : DbContext
    {
       
        public WFHContext()
            : base("name=WFHContext")
        {
        }
        
       
    }

  
}