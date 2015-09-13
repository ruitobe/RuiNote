using System;

namespace RuiNote.Services.Model
{
    public class Note
    {
        /// <summary>
        /// Gets or sets the note title.
        /// </summary>
        /// <value>
        /// The note title.
        /// </value>
        public string NoteTitle { get; set; }

        /// <summary>
        /// Gets or sets the note content.
        /// </summary>
        /// <value>
        /// The note content.
        /// </value>
        public string NoteContent { get; set; }

        /// <summary>
        /// Gets or sets the note created time.
        /// </summary>
        /// <value>
        /// The note created time.
        /// </value>
        public DateTime NoteTime { get; set; }

        /// <summary>
        /// Gets or sets the note id.
        /// </summary>
        /// <value>
        /// The note id.
        /// </value>
        public int ID
        {
            get;
            set;
        }
    }

}
