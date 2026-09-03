using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class GroupDirectReport
    {
        public List<M_Level> m_Levels { get; set; }
        public List<M_LevelDetail> m_LevelDetails { get; set; }
        public List<ReferalDownlinein> referalDownlineins { get; set; }
        public IPagedList<int> pagerCount { get; set; }
        public List<M_LevelIncome> m_LevelIncomes { get; set; }
    }

    public class M_Level
    {
        public decimal MLevel { get; set; }
        public string LevelName { get; set; }
    }

    public class M_LevelDetail
    {
        public Int64 SNo { get; set; }
        public string Sponsorid { get; set; }
        public string MemberName { get; set; }
        public string Idno { get; set; }
        public string MemName { get; set; }
        public int Mlevel { get; set; }
        public string Doj { get; set; }
        public string UpgradeDate { get; set; }
        public decimal PV { get; set; }
        public string Packagename { get; set; }
        public decimal BV { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }
    }
    public class ReferalDownlinein
    {
        public string FormNo { get; set; }
        public int RegisterLeft { get; set; }
        public int RegisterRight { get; set; }
        public int ConfirmLeft { get; set; }
        public int ConfirmRight { get; set; }
        public decimal LeftBv { get; set; }
        public decimal RightBv { get; set; }
    }

    public class M_LevelIncome
    {
        public int SNo { get; set; }
        public string Date { get; set; }
        public string Level { get; set; }
        public string MemberName { get; set; }
        public string Business { get; set; }
        public string Slab { get; set; }
        public string LevelBonus { get; set; }
        public string Idno { get; set; }
    }
}