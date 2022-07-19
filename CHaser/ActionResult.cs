using System.Collections;

namespace DoctorFe.CHaser
{
    /// <summary>
    /// サーバー側の応答の構造体
    /// </summary>
    public struct ActionResult : IEnumerable<int>
    {

        /// <summary>
        /// ゲームがまだ有効であるかを返します。
        /// </summary>
        public bool WillContinue {get; private set;}
        private int[] data;

        /// <summary>
        /// IEnumerable を使わないでアクセスします。
        /// </summary>
        public int this[int i]
        {
            get {
                return data[i];
            }
        }

        internal ActionResult(int[] data, bool willContinue)
        {
            WillContinue = willContinue;
            this.data = data;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return data.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}