using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using loveproject.dataa.Abstract;
using loveproject.entityy;

namespace loveproject.dataa.Abstract
{
    public interface ILoverMatchRepository:IRepository<LoverMatch>
    {

        public List<LoverMatch> Find(string Any);

    }
}
