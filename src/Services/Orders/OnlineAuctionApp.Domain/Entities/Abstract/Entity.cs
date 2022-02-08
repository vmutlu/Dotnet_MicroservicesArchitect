using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAuctionApp.Domain.Entities.Abstract
{
    public abstract class Entity : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; protected set; }

        /*
         * resource: https://www.google.com/search?q=membervise+copy&oq=membervise+copy&aqs=chrome..69i57j0i19j0i19i22i30l2j0i5i13i19i30j0i8i13i19i30j0i19i22i30l4.4976j1j1&sourceid=chrome&ie=UTF-8
         */

        //Shallow copy 
        public Entity Clone() => (Entity)this.MemberwiseClone();
    }
}
