using CodeOverFlow.Data;
using CodeOverFlow.Entities;

namespace CodeOverFlow.Services
{
    public class TagService
    {
        private readonly TagRepository _tagRepository = new TagRepository();

        public int GetOrCreateTagId(string tagName)
        {
            var existingTagId = _tagRepository.GetIdByName(tagName);
            if (existingTagId.HasValue)
                return existingTagId.Value;
            return _tagRepository.AddTag(tagName); // returns new tag ID
        }
        public List<Tag> GetAll() => _tagRepository.GetAll();
    }
}
