using System.IO;

namespace DATA
{
    /// <summary>
    /// Интерфейс используемый для пометки класса который нужно сериализовать в JSONContainer 
    /// </summary>
    public interface IAddition
    {

#if UNITY_EDITOR
        /// <summary>
        /// Используется для отрисовки в окне инспектора
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// Экспорт данных в файл для использование на сервере
        /// </summary>
        /// <param name="stream_out"></param>
        public abstract void Export(BinaryWriter stream_out);
        
#endif
    }
}
