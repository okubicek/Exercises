using System;
using System.Linq;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using Lucene.Net.Codecs;
using Lucene.Net.Index;
using Lucene.Net;
using System.Text.Json;

namespace lucene
{
    public class Program
    {
        static void Main(string[] args)
        {
            var jsonProps = "[{\"name\":\"Ondrej\",\"surname\":\"Kubicek\",\"app_data\":[\"112233\", \"56612\"]},"
            + "{\"name\":\"Lukas\",\"surname\":\"Bily\",\"app_data\":[\"12355\", \"112233\", \"89466\"]}," + 
            "{\"name\":\"Lenak\",\"surname\":\"Nejaka\",\"app_data\":[\"89700\"]}]";

            var version = LuceneVersion.LUCENE_48;
            var dir = new RAMDirectory();

            var analyzer = new StandardAnalyzer(version);
            var indexConfig = new IndexWriterConfig(version, analyzer);

            var writer = new IndexWriter(dir, indexConfig);
                        
            var d = JsonDocument.Parse(jsonProps);
            var root = d.RootElement;

            foreach(var line in root.EnumerateArray())
            {
                var doc = new Document();

                doc.Add(new StringField("name", line.GetProperty("name").GetString(), Field.Store.NO));
                doc.Add(new StringField("surname", line.GetProperty("surname").GetString(), Field.Store.NO));
                foreach(var f in line.GetProperty("app_data").EnumerateArray())
                {
                    doc.Add(new StringField("app_data", f.GetString(), Field.Store.NO));
                }

                doc.Add(new StringField("payload", line.ToString(), Field.Store.YES));

                writer.AddDocument(doc);                
                // Console.WriteLine(line.GetProperty("name"));
                // if (line.GetProperty("app_data").EnumerateArray().Any(x => x.GetString() == "1"))
                // {

                // }

                // foreach(var data in line.GetProperty("app_data").EnumerateArray())
                // {
                // }
                
                // Console.WriteLine(line.GetProperty("app_data").GetArrayLength());                
            }            

            writer.Flush(false, false);

            var searcher = new IndexSearcher(writer.GetReader(true));
            
            var query= new MultiPhraseQuery();
            query.Add(new Term("app_data", "12355"));

            var booleanQuery = new BooleanQuery();
            booleanQuery.Add(new TermQuery(new Term("app_data", "12355")), Occur.SHOULD);
            booleanQuery.Add(new TermQuery(new Term("app_data", "89700")), Occur.SHOULD);

            var res = searcher.Search(booleanQuery, 100);

            Console.WriteLine(res.TotalHits);
            foreach(var hit in res.ScoreDocs)
            {
                var item = searcher.Doc(hit.Doc);
                Console.WriteLine(item.Get("payload"));
            }
        }
    }
}
