namespace DtoTools
{
    class DtoPropertyInfo
    {
        public string Description { get; set; }
        public bool IsNullable { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return $"{Type} {Name} // {Description}";
        }
    }
}
