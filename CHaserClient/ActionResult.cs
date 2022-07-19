using System.Collections;

namespace DoctorFe.CHaser
{
    /// <summary>
    /// サーバー側の応答の構造体
    /// </summary>
    public struct ActionResult : IEnumerable
    {

        /// <summary>
        /// ゲームがまだ続くかを返します。
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

        /// <summary>
        /// プロジェクト内でのみ使用するコンストラクタです。
        /// </summary>
        /// <param name="data">サーバーからの応答</param>
        /// <param name="willContinue">ゲームが続くか</param>
        internal ActionResult(int[] data, bool willContinue)
        {
            WillContinue = willContinue;
            this.data = data;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}