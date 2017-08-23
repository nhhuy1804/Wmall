using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.TableLayout.PI
{
    public interface ITLP001Presenter
    {
        void GetTable(int floor);
        void UpdateTable(int id, int x, int y);
        void DeleteTable(int id);
        void InsertTable(int index, int x, int y, int floor);
        int MaxIndexTable(int floor);
    }
}
