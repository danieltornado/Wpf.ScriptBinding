using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ScriptBinding.Debugger.ErrorListeners;
using ScriptBinding.Debugger.ViewModels.Base;
using ScriptBinding.Internals.Compiler;
using ScriptBinding.Internals.Compiler.Expressions;
using ScriptBinding.Internals.Executor;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Debugger.ViewModels
{
    sealed class MainViewModel : ViewModelBase, IBindingGenerator, IBindingProvider
    {
        public MainViewModel()
        {
            Bindings = new ObservableCollection<BindingViewModel>();
            _run = new RelayCommand(RunExecute, RunCanExecute);
        }

        private Expr CompiledExpression { get; set; }

        private string _expression;
        public string Expression
        {
            get => _expression;
            set
            {
                _expression = value;
                CreateTrees(value);
            }
        }
        
        #region Run

        private readonly RelayCommand _run;
        public ICommand Run => _run;

        private void RunExecute()
        {
            using (var writer = new StringWriter())
            {
                var result = Execute(Expression, CompiledExpression, writer, this);
                writer.WriteLine(result);

                writer.Flush();

                Output = writer.ToString();
            }

            RaisePropertyChanged(nameof(Output));
        }

        private bool RunCanExecute()
        {
            return CompiledExpression != null;
        }

        #endregion

        public string Output { get; private set; }

        public ObservableCollection<ExprViewModel> CompilerTree { get; private set; }

        public ObservableCollection<NodeViewModel> ParserTree { get; private set; }

        public ObservableCollection<BindingViewModel> Bindings { get; }

        private void CreateTrees(string expression)
        {
            ClearBindings();

            using (var writer = new StringWriter())
            {
                var parserTreeModel = Parse(expression, writer);
                ParserTree = new ObservableCollection<NodeViewModel> { new NodeViewModel(parserTreeModel) };

                CompiledExpression = Compile(parserTreeModel, this);
                CompilerTree = new ObservableCollection<ExprViewModel> { new ExprViewModel(CompiledExpression) };

                writer.Flush();

                Output = writer.ToString();
            }

            RaisePropertyChanged(nameof(ParserTree));
            RaisePropertyChanged(nameof(CompilerTree));
            RaisePropertyChanged(nameof(Output));
        }

        private void ClearBindings()
        {
            Bindings.Clear();
        }

        private static Node Parse(string expression, TextWriter output)
        {
            var errorListener = new ParserErrorListener(expression, output);
            var parser = new Internals.Parser.Parser(errorListener);
            var result = parser.Parse(expression);

            return result;
        }

        private static Expr Compile(Node node, IBindingGenerator bindingGenerator)
        {
            if (node is null)
                return null;

            var compiler = new Compiler(bindingGenerator);
            var expr = compiler.Compile(node);

            return expr;
        }

        private static object Execute(string expression, Expr expr, TextWriter output, IBindingProvider bindingProvider)
        {
            var errorListener = new ExecutorErrorListener(expression, output);
            var executor = new Executor(errorListener, bindingProvider);
            var result = executor.Execute(expr);

            return result;
        }

        private IBindingValueViewModel FindValueSource(int index)
        {
            if (index >= 0 && index < Bindings.Count)
                return Bindings[index].ValueViewModel;

            return default;
        }

        private IBindingValueViewModel FindValueSource(string propertyPath)
        {
            var bindingViewModel = Bindings.Where(e => !(e is ElementBindingViewModel)).FirstOrDefault(e => e.PropertyPath == propertyPath);
            if (bindingViewModel != null)
                return bindingViewModel.ValueViewModel;

            return default;
        }

        private IBindingValueViewModel FindValueSource(string propertyPath, string elementName)
        {
            var bindingViewModel = Bindings.OfType<ElementBindingViewModel>().FirstOrDefault(e => e.PropertyPath == propertyPath && e.ElementName == elementName);
            if (bindingViewModel != null)
                return bindingViewModel.ValueViewModel;

            return default;
        }

        private bool TryGetValue(IBindingValueViewModel source, out object value)
        {
            if (source == null)
            {
                value = null;
                return false;
            }

            value = source.HasErrors() ? null : source.GetValue();
            return true;
        }

        #region Implementation of IBindingGenerator

        /// <inheritdoc />
        void IBindingGenerator.GenerateBinding(string propertyPath)
        {
            var binding = new BindingViewModel(propertyPath);
            Bindings.Add(binding);
        }

        /// <inheritdoc />
        void IBindingGenerator.GenerateBinding(string propertyPath, string elementName)
        {
            var binding = new ElementBindingViewModel(propertyPath, elementName);
            Bindings.Add(binding);
        }

        #endregion

        #region Implementation of IBindingProvider

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(int index, out object value)
        {
            var source = FindValueSource(index);
            return TryGetValue(source, out value);
        }

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(string propertyPath, out object value)
        {
            var source = FindValueSource(propertyPath);
            return TryGetValue(source, out value);
        }

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(string propertyPath, string elementName, out object value)
        {
            var source = FindValueSource(propertyPath, elementName);
            return TryGetValue(source, out value);
        }

        #endregion
    }
}
