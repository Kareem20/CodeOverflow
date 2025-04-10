using CodeOverFlow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
