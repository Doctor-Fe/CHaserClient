using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DoctorFe.CHaser
{
    /// <summary>
    /// CHaser のクライアント側のクラス
    /// </summary>
    public class Client
    {
        #region メンバー変数とプロパティ
            public bool IsRunning {get; private set;}

            public IPAddress Address {get; private set;}
            public int Port {get; private set;}
            public string Name {get; private set;}

            private Socket socket;
        #endregion

        private static char[] getReady = {'g', 'r', '\r', '\n'};
        
        /// <summary>
        /// サーバーとの通信を確立させ、ユーザー名を送信します。
        /// </summary>
        public Client(IPAddress address, int port, string name)
        {
            this.Address = address;
            this.Port = port;
            this.Name = name;
            this.IsRunning = false;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(Address, Port);
                if (socket.Connected)
                {
                    socket.Send(Encoding.UTF8.GetBytes(Name + "\r\n"));
                    IsRunning = true;
                }
            } catch(SocketException)
            {
                throw;
            }
        }

        /// <summary>
        /// 動作を指定し、サーバー側に送信します。
        /// <param name="type">動作の種類を指定します。</param>
        /// <param name="dir">動作をする方向を指定します。</param>
        /// <returns>サーバーからの応答</returns>
        /// </summary>
        public ActionResult DoAction(ActionType type, Direction dir)
        {
            return Input(string.Format("{0}{1}", (char)type, (char)dir));
        }

        /// <summary>
        /// サーバーに GetReady を送信します。
        /// </summary>
        /// <returns>サーバーの応答</returns>

        public ActionResult GetReady()
        {
            try
            {
                byte[] buffer = new byte[100];
                socket.Receive(buffer); // 意味不明。ダミー?
                socket.Send(Encoding.UTF8.GetBytes(getReady));
                int dataSize = socket.Receive(buffer);
                byte[] data = new byte[dataSize];
                int[] ret = Convert(buffer[0..dataSize]);
                if (ret[0] == 0)
                {
                    Close();
                    return new ActionResult(ret[1..^0], false);
                } else
                {
                    return new ActionResult(ret[1..^0], true);
                }
            } catch (Exception)
            {
                if (!socket.Connected)
                {
                    Close();
                    return new ActionResult(new int[]{-1, -1, -1, -1, -1, -1, -1, -1, -1}, false);
                } else
                {
                    IsRunning = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// 実際に送信するメソッドはこれです。
        /// </summary>
        /// <param name="input">送信する内部的な文字列。</param>
        /// <returns>サーバーからの応答</returns>
        private ActionResult Input(string input)
        {
            try
            {
                byte[] buffer = new byte[100];
                socket.Send(Encoding.UTF8.GetBytes(input+"\r\n"));
                int dataSize = socket.Receive(buffer);
                byte[] data = new byte[dataSize];
                int[] ret = Convert(buffer[0..dataSize]);
                socket.Send(Encoding.UTF8.GetBytes("#\r\n"));
                if (ret[0] == 0)
                {
                    Close();
                    return new ActionResult(ret[1..^0], false);
                } else
                {
                    return new ActionResult(ret[1..^0], true);
                }
            } catch (Exception)
            {
                if (!socket.Connected)
                {
                    Close();
                    return new ActionResult(new int[]{-1, -1, -1, -1, -1, -1, -1, -1, -1}, false);
                } else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// サーバーとの通信を終了します。ふつうは内部でのみ使用します。
        /// </summary>
        public void Close()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            IsRunning = false;
        }

        /// <summary>
        /// サーバーからの応答を、クライアント側で扱えるように変換します。
        /// </summary>
        /// <param name="data">サーバーからの生の応答</param>
        /// <returns>変換済みのデータ</returns>
        private static int[] Convert(byte[] data)
        {
            int[] result = new int[data.Length - 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = data[i] - 48;
            }
            return result;
        }
    }
}