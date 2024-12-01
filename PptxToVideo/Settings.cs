using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PptxToVideo
{
    public class Settings
    {
        public string RecentElevenLabsApiKey { get; set; }
        public ObservableCollection<string> ElevenLabsApiKeys { get; set; } = new ObservableCollection<string>();
        public Voice SelectedVoice { get; set; }

    }
}
