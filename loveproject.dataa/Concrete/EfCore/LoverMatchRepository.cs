using loveproject.dataa.Abstract;
using loveproject.entityy;
using loveproject.dataa.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loveproject.dataa.Concrete.EfCore
{
    public class LoverMatchRepository : GenericEfCoreRepository<LoverMatch,EfCoreContext>, ILoverMatchRepository
    {
        public List<LoverMatch> Find(string Any)
        {
            using(var context = new EfCoreContext())
            {
                List<LoverMatch>? value = context.LoverMatches.Where(i => i.Name1.Contains(Any)).ToList();
                if(value.Count==0)
                {
                    value = context.LoverMatches.Where(i => i.Name2.Contains(Any)).ToList();

                }if(value.Count==0)
                {
                    value= context.LoverMatches.Where(i => i.Score.Contains(Any)).ToList();

                }

                return value;

            }
        }
    }
}
