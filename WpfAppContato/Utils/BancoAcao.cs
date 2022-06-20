using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfAppContato.Utils
{
    public class BancoAcao
    {
        private static volatile BancoAcao fObjeto;
        private static object syncRoot = new Object();

        public static BancoAcao Instance
        {
            get
            {
                if (fObjeto == null)
                {
                    lock (syncRoot)
                    {
                        if (fObjeto == null)
                        {
                            fObjeto = new BancoAcao();
                        }
                    }
                }
                return fObjeto;
            }
        }

        public List<Contato> RetornarLista()
        {
            BDContatosEntities contexto = new BDContatosEntities();
            return contexto.Set<Contato>().ToList();
        }

        public bool IncluirItem(Contato dados)
        {
            using (BDContatosEntities contexto = new BDContatosEntities())
            {
                if (contexto.Set<Contato>().Any(p => p.Email.Trim().ToUpper().Equals(dados.Email.Trim().ToUpper())))
                {
                    return false;
                }
                else
                {
                    contexto.Set<Contato>().Add(dados);
                    var retorno = contexto.SaveChanges();
                    return retorno > 0;
                }
            }
        }

        public bool AtualizarItem(Contato dados)
        {
            using (BDContatosEntities contexto = new BDContatosEntities())
            {
                if (dados.Id > 0)
                {
                    var item = contexto.Set<Contato>().Where(p => p.Id == dados.Id).FirstOrDefault();
                    if (item != null)
                    {
                        item.Nome = dados.Nome;
                        item.Email = dados.Email;
                        item.Telefone = dados.Telefone;
                        contexto.Contato.Attach(item);
                        contexto.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        var retorno = contexto.SaveChanges();
                        return true;
                    }
                }
            }
            return true;
        }

        public Contato RetornarItemPorNome(string nome)
        {
            using (BDContatosEntities contexto = new BDContatosEntities())
            {
                var item = contexto.Set<Contato>().Where(p => p.Nome.Trim().ToUpper().Contains(nome.Trim().ToUpper())).FirstOrDefault();
                return item;
            }
        }

        public bool RemoverItem(string nome)
        {
            using (BDContatosEntities contexto = new BDContatosEntities())
            {
                var item = contexto.Set<Contato>().Where(p => p.Nome.Trim().ToUpper().Contains(nome.Trim().ToUpper())).FirstOrDefault();
                if (item.Id > 0)
                {
                    contexto.Set<Contato>().Remove(item);
                    contexto.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public List<Contato> RetornarlistaOrdenada(bool ordenacaoAcendente)
        {
            using (BDContatosEntities contexto = new BDContatosEntities())
            {
                if (ordenacaoAcendente)
                {
                    return contexto.Set<Contato>().OrderBy(p => p.Nome).ToList();
                }
                else
                {
                    return contexto.Set<Contato>().OrderByDescending(p => p.Nome).ToList();
                }
            }

        }
    }
}
