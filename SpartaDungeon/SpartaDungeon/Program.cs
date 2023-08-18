using System.Reflection.PortableExecutable;

namespace SpartaDungeon
{
    internal class Program
    {
        private static PlayerStat ps;
        static Dictionary<string, List<Inventory>> inventoryDict = new Dictionary<string, List<Inventory>>();
        static void Main(string[] args)
        {
            GameDataSetting();
            GameStart();

        }

        static void GameDataSetting()
        {
            ps = new PlayerStat(1, "Munch", 10, 5, 100, 500);

            List<Inventory> inventoryList = new List<Inventory>();
            
            inventoryList.Add(new Inventory("낡은 검", "무기", "흔하게 보이는 검이다.", 2, 0));
            inventoryList.Add(new Inventory("라브리스", "무기", "이 양날 전투 도끼는 적을 박살 낼 것이다.", 10, 0));

            inventoryDict["Inventory"] = inventoryList;

        }

        static public void GameStart()
        {

            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 행동을 선택하세요!\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine(" ");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n >>");
            int _action = CheckValidAction(1, 5);
            switch (_action)
            {
                case 1:
                    ViewStat();
                    break;
                case 2:
                    ViewInventory();
                    break;
            }

        }

        static void ViewStat()
        {
            Console.Clear();
            Console.WriteLine($"{ps.Name}님의 인벤토리\n");
            Console.WriteLine($" 레벨 : {ps.Level}");
            Console.WriteLine($" 이름 : {ps.Name}");
            Console.WriteLine($" 공격력 : {ps.AtkValue}");
            Console.WriteLine($" 방어력 : {ps.DefValue}");
            Console.WriteLine($" 체력 : {ps.HpValue}");
            Console.WriteLine($" 골드 : {ps.Gold} G");
            Console.WriteLine(" ");
            Console.WriteLine("0. 돌아가기");

            int _action = CheckValidAction(0, 0);
            switch (_action)
            {
                case 0:
                    GameStart();
                    break;
            }
        }

        static void ViewInventory()
        {
            Console.Clear();
            Console.WriteLine($"{ps.Name}님의 인벤토리\n");

            List<Inventory> viewInventory = inventoryDict["Inventory"];


            if (viewInventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어있습니다.");
            }
            else
            {
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine(new string('-', 13)); // 구분선 출력

                foreach (var item in viewInventory)
                {
                    Console.WriteLine($"| {item.ItemName} | {item.ItemType} | {item.ItemComm} | 공격력 + {item.ItemAtkValue} | 방어력 + {item.ItemDefValue} |");
                }
            }


            Console.WriteLine("1. 관리하기");
            Console.WriteLine("0. 돌아가기");

            int _action = CheckValidAction(0, 1);
            switch (_action)
            {
                case 0:
                    GameStart();
                    Console.WriteLine();
                    break;
                case 1:
                    Console.Clear();
                    ManagementInventory();
                    break;

            }
        }


        static void ManagementInventory()
        {
            Console.Clear();
            Console.WriteLine($"{ps.Name}님의 인벤토리 관리\n");
            Console.WriteLine($"아이템을 장착하시려면, 아이템을 선택해주세요!");
            List<Inventory> ManagementInventory = inventoryDict["Inventory"];

            if (ManagementInventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어있습니다.");
            }
            else
            {
                int equippedIndex = -1; // 초기값으로 장착된 아이템이 없음을 나타냄
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine(new string('-', 13)); // 구분선 출력

                for (int i = 0; i < ManagementInventory.Count; i++)
                {
                    var item = ManagementInventory[i];
                    Console.WriteLine($"| {i + 1} | {item.ItemName} | {item.ItemType} | {item.ItemComm} | 공격력 + {item.ItemAtkValue} | 방어력 + {item.ItemDefValue} |");
                }

                Console.WriteLine("번호를 선택하세요: ");
                int _selectItem = CheckValidAction(1, ManagementInventory.Count);
                switch (_selectItem)
                {
                    case 1:
                        break;
                }
            }

            Console.WriteLine("0. 돌아가기");

            int _action = CheckValidAction(0, 1);
            switch (_action)
            {
                case 0:
                    ViewInventory();
                    Console.WriteLine();
                    break;
                case 1:
                    
                    Console.WriteLine();
                    break;

            }
        }

        static int CheckValidAction(int _min, int _max)
        {
            while (true)
            {
                string _action = Console.ReadLine();

                bool _parseSuccess = int.TryParse(_action, out var _ret);
                if (_parseSuccess)
                {
                    if (_ret >= _min && _ret <= _max)
                        return _ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        public class Inventory
        {
            public string ItemName { get; }
            public string ItemType { get; }
            public string ItemComm { get; }
            public int ItemAtkValue { get; }
            public int ItemDefValue { get; }

            public Inventory(string _itemName, string _itemType, string _itemComm, int _itemAtkValue, int _itemDefValue)
            {
                ItemName = _itemName;
                ItemType = _itemType;
                ItemComm = _itemComm;
                ItemAtkValue = _itemAtkValue;
                ItemDefValue = _itemDefValue;
            }
        }

        

        public class PlayerStat
        {
            public int Level { get; }
            public string Name { get; }
            public int AtkValue { get; set; }
            public int DefValue { get; set; }
            public int HpValue { get; }
            public int Gold { get; }

            public PlayerStat(int _level, string _name, int _atkValue, int _defValue, int _hpValue, int _gold)
            {
                Level = _level;
                Name = _name;
                AtkValue = _atkValue;
                DefValue = _defValue;
                HpValue = _hpValue;
                Gold = _gold;
            }
        }
    }
}