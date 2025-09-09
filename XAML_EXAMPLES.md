<!-- Simple example of moving elements around -->
<StackPanel Spacing="15" Margin="20">
    <!-- Move this button by changing Margin -->
    <Button Content="Top Button" Margin="0,0,0,10"/>
    
    <!-- This text is centered - change alignment -->
    <TextBlock Text="Centered Text" 
               HorizontalAlignment="Center"
               FontSize="18"/>
    
    <!-- Change the order to move elements up/down -->
    <Button Content="Bottom Button"/>
</StackPanel>

<!-- Grid example for precise positioning -->
<Grid RowDefinitions="Auto,*,Auto" ColumnDefinitions="200,*">
    <!-- This stays at top -->
    <TextBlock Grid.Row="0" Grid.Column="0" Text="Label"/>
    
    <!-- This fills middle space -->
    <TextBox Grid.Row="1" Grid.Column="1" Text="Content"/>
    
    <!-- This stays at bottom -->
    <Button Grid.Row="2" Grid.Column="1" Content="Submit"/>
</Grid>