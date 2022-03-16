# Wpf.ScriptBinding
Use binding for complex path with dynamic expression.

# Sample

``` xaml
<!-- creates binding to the property Text -->
<TextBlock Text="{scriptBinding:ScriptBinding 'b(Text)'}" />

<!-- creates binding to the property Number and invokes ToString on the binding value -->
<TextBlock Text="{scriptBinding:ScriptBinding 'b(Number).ToString()'}" />

<!-- creates binding to the control MainGrid and to his property ActualWidth, returns binding value divided by 2 -->
<Rectangle Width="{scriptBinding:ScriptBinding 'b(ActualWidth, MainGrid) / 2'}" />

<!-- creates binding to the control TextInputVisibility and to his property IsChecked, checks the binding value and returns condition -->
<TextBox Visibility="{scriptBinding:ScriptBinding 'if(b(IsChecked, TextInputVisibility) == true) then(System.Windows.Visibility.Visible) else(System.Windows.Visibility.Collapsed)'}" />

<!-- does not create bindings, but use the internal binding collection and gets binding by index -->
<scriptBinding:ScriptBinding Expression="b(0) + b(1)">
  <Binding Path="Text" />
  <Binding Path="TwoWayBindingText" />
</scriptBinding:ScriptBinding>
```
