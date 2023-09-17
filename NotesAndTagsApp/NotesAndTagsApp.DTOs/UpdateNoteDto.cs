﻿using NotesAndTagsApp.Domain.Enum;

namespace NotesAndTagsApp.DTOs
{
    public class UpdateNoteDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public PriorityEnum Priority { get; set; }
        public TagEnum Tag { get; set; }    
        public int UserId { get; set; }
    }
}
