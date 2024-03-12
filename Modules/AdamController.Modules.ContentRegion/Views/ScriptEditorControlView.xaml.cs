using System;
using System.Windows.Controls;


namespace AdamController.Modules.ContentRegion.Views
{
    public partial class ScriptEditorControlView : UserControl
	{
		public ScriptEditorControlView()
		{
			InitializeComponent();
			//LoadHighlighting();

			//TextResulEditor.TextChanged += TextResulEditorTextChanged;
		}

		private void TextResulEditorTextChanged(object sender, EventArgs e)
		{
			//Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
			//{
			//	TextResulEditor.ScrollToEnd();
			//}));
	
		}
		
		#region Example code

			/*public ScriptEditorControl()
			{
				//IHighlightingDefinition customHighlighting = HighlightingManager.Instance.HighlightingDefinitions[0];

				InitializeComponent();

				//textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
				//textEditor.SyntaxHighlighting = customHighlighting;
				// initial highlighting now set by XAML

				//textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
				//textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

				/*DispatcherTimer foldingUpdateTimer = new()
				{
					Interval = TimeSpan.FromSeconds(2)
				};

				foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
				foldingUpdateTimer.Start();
			}
			string currentFileName;

			void openFileClick(object sender, RoutedEventArgs e)
			{
				OpenFileDialog dlg = new()
				{
					CheckFileExists = true
				};

				if (dlg.ShowDialog() ?? false)
				{
					currentFileName = dlg.FileName;
					textEditor.Load(currentFileName);
					textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
				}
			}

			void saveFileClick(object sender, EventArgs e)
			{
				if (currentFileName == null)
				{
					SaveFileDialog dlg = new SaveFileDialog();
					dlg.DefaultExt = ".txt";
					if (dlg.ShowDialog() ?? false)
					{
						currentFileName = dlg.FileName;
					}
					else
					{
						return;
					}
				}
				textEditor.Save(currentFileName);
			}



			CompletionWindow completionWindow;

			void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
			{
				if (e.Text == ".")
				{
					// open code completion after the user has pressed dot:
					completionWindow = new CompletionWindow(textEditor.TextArea);

					// provide AvalonEdit with the data:
					IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;

					data.Add(new EditorModel("Item1"));
					data.Add(new EditorModel("Item2"));
					data.Add(new EditorModel("Item3"));
					data.Add(new EditorModel("Another item"));
					completionWindow.Show();
					completionWindow.Closed += delegate {
						completionWindow = null;
					};
				}
			}

			void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
			{
				if (e.Text.Length > 0 && completionWindow != null)
				{
					if (!char.IsLetterOrDigit(e.Text[0]))
					{
						// Whenever a non-letter is typed while the completion window is open,
						// insert the currently selected element.
						completionWindow.CompletionList.RequestInsertion(e);
					}
				}
				// do not set e.Handled=true - we still want to insert the character that was typed
			}

			#region Folding
			private FoldingManager foldingManager;
			private XmlFoldingStrategy foldingStrategy;

			void HighlightingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
			{
				if (textEditor.SyntaxHighlighting == null)
				{
					foldingStrategy = null;
				}
				else
				{
					switch (textEditor.SyntaxHighlighting.Name)
					{
						case "XML":
							foldingStrategy = new XmlFoldingStrategy();
							textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
							break;
						case "C#":
						case "C++":
						case "PHP":
						case "Java":
							textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(textEditor.Options);
							foldingStrategy = new XmlFoldingStrategy();
							break;
						default:
							textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
							foldingStrategy = null;
							break;
					}
				}
				if (foldingStrategy != null)
				{
					if (foldingManager == null)
						foldingManager = FoldingManager.Install(textEditor.TextArea);

					foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
				}
				else
				{
					if (foldingManager != null)
					{
						FoldingManager.Uninstall(foldingManager);
						foldingManager = null;
					}
				}
			}

			void foldingUpdateTimer_Tick(object sender, EventArgs e)
			{
				if (foldingStrategy != null)
				{
					foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
				}
			}

		}*/

			#endregion
	}
}
