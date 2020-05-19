using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
namespace WebApi.Common.FileAdapter
{
    public class FileBase 
    {
        public List<string> ListError = new List<string>();
        public virtual IQueryable<PosData> Parse(int _Model,string _FilePath)
        {
            IQueryable<PosData> _PosList = new List<PosData>().AsQueryable();
            return _PosList;
        }
        public virtual IQueryable<Pos> ParsePos(int _Model, string _FilePath)
        {
            IQueryable<Pos> _PosList = new List<Pos>().AsQueryable();
            return _PosList;
        }
        public virtual IQueryable<Edi_Pos> ParseEdiPos( string _FilePath)
        {
            IQueryable<Edi_Pos> _Edi_PosList = new List<Edi_Pos>().AsQueryable();
            return _Edi_PosList;
        }
    }
}