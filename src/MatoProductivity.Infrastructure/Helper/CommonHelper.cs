using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Infrastructure.Helper
{
    public class CommonHelper
    {
        public static int[] GetRandomArry(int minval, int maxval)
        {

            int[] arr = new int[maxval - minval + 1];
            int i;
            //初始化数组
            for (i = 0; i <= maxval - minval; i++)
            {
                arr[i] = i + minval;
            }
            //随机数
            Random r = new Random();
            for (int j = maxval - minval; j >= 1; j--)
            {
                int address = r.Next(0, j);
                int tmp = arr[address];
                arr[address] = arr[j];
                arr[j] = tmp;
            }
            //输出
            foreach (int k in arr)
            {
                Debug.WriteLine(k + " ");
            }
            return arr;
        }


    }
}
