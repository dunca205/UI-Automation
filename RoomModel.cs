namespace AutomationInTesting
{
    public class RoomModel
    {
        public required string RoomName { get; set; }

        public string RoomType { get; set; }

        public string IsAccessible { get; set; }

        public required string Price { get; set; }

        public List<string> RoomDetails { get; set; }
    }
}
