using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Shared.Data
{
    public class MenuData
    {
        public MenuData()
        {
            Visible = true;
        }

        public string MenuId { get; set; }
        public int MenuSeq { get; set; }
        public string MenuName { get; set; }
        public string MenuClass { get; set; }
        public string MenuDll { get; set; }
        public string PMenuId { get; set; }
        public MenuType MenuType { get; set; }
        public bool Visible { get; set; }

        /// <summary>
        /// 관리자용 메뉴
        /// </summary>
        public bool IsAdminMenu { get; set; }

        /// <summary>
        /// 디버그모드사용
        /// </summary>
        public bool TestMenu { get; set; }
        public string MenuKey { get; set; }

        /// <summary>
        /// Menu click validation
        /// </summary>
        public string ValidatorClass { get; set; }
    }
}
