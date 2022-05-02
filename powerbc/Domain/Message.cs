namespace powerbc.Domain
{
    public class Message
    {
        public string Id { get; init; }

        public User Sender { get; init; }

        private string _content = "";
        public string Content
        { 
            get => _content;
            
            // Content 可修改/編輯，並同時更新修改時間
            set 
            {
                _content = value;
                _time = DateTime.Now;
            } 
        }

        private DateTime _time = DateTime.Now;
        public DateTime Time
        {
            get => _time;
        }

        public Message(string Id, User sender, string content)
        {
            this.Id = Id;
            _content = content;
            Sender = sender;
        }
    }
}
