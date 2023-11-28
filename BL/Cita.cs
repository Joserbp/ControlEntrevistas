using ML;

namespace BL
{
    public class Cita
    {
        public ML.Result Add(ML.Cita cita)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
                {
                    context.Add(cita);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false; 
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}