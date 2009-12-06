namespace Effectus.Features.Merge
{
	using Effectus.Model;
	using Infrastructure;

	public class Presenter : AbstractPresenter<Model, View>
	{
		public void Initialize(ToDoAction userVersion)
		{
			using(var tx = Session.BeginTransaction())
			{
				Model = new Model
				{
					UserVersion = userVersion,
					DatabaseVersion = Session.Get<ToDoAction>(userVersion.Id),
                    AllowEditing = new Observable<bool>(false)
				};	

				tx.Commit();
			}
		}

		public void OnAcceptDatabaseVersion()
		{
			// nothing to do
			Result = MergeResult.AcceptDatabaseVersion;
			View.Close();
		}

		public void OnForceUserVersion()
		{
			using(var tx = Session.BeginTransaction())
			{
				//updating the object version to the current one
				Model.UserVersion.Version = Model.DatabaseVersion.Version;
				Session.Merge(Model.UserVersion);
				tx.Commit();
			}
			Result = MergeResult.ForceDatabaseVersion; 
			View.Close();
		}
	}
}