using AnaliseDadosDotNetCore.DAL.Context;
using AnaliseDadosDotNetCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnaliseDadosDotNetCore.DAL
{
    class DadosDal
    {
        public IList<TbCoronaVirus> Listar()
        {
            DbApiContext ctx = new DbApiContext();
            IList<TbCoronaVirus> lista = new List<TbCoronaVirus>();
            lista = ctx.tbCoronaVirus.ToList<TbCoronaVirus>();
            return lista;
        }

        public TbCoronaVirus Consultar(int id)
        {
            DbApiContext ctx = new DbApiContext();
            TbCoronaVirus coronaVirus = ctx.tbCoronaVirus.Find(id);
            return coronaVirus;
        }

        public void Inserir(TbCoronaVirus tbCoronaVirus)
        {
            DbApiContext ctx = new DbApiContext();
            ctx.tbCoronaVirus.Add(tbCoronaVirus);
            ctx.SaveChanges();
        }

        public void Alterar(TbCoronaVirus tbCoronaVirus)
        {
            DbApiContext ctx = new DbApiContext();
            ctx.Entry(tbCoronaVirus).State = EntityState.Modified; 
            ctx.tbCoronaVirus.Update(tbCoronaVirus);
            ctx.SaveChanges();
        }

        public void Excluir(int id)
        {
            DbApiContext ctx = new DbApiContext();
            TbCoronaVirus coronaVirus = ctx.tbCoronaVirus.Find(id);
            ctx.Entry(coronaVirus).State = EntityState.Deleted;
            ctx.SaveChanges();
        }

    }
}
