   M S B A M L   `   `   `  ����  PLAYLIST    PLAYLIST.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  My Playlist��5   	   $	��450��6   $	��800��6*   ���	#FF121212.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5   
   �� $��My Playlist��6   $��24(�6'   $
��Bold"�65   ���#9f61e2$
��Left=�5      $	��Top=�6/   $��45,6,0,0q�6G   $��2��6Y   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $	��Top=�6J   $��0��6b   .+�5      �� -   5        $��Mis Playlists��5      ���	#FF1DB954
�������$
��10,5q�6|   $��5q�6�   �� 5      -   5        $��Shuffle��5      ���	#FF1DB954
�������$
��10,5q�6i   $��5q�6x   �� 5      -   5        $��Add Song��5      ���	#FF1DB954
�������$
��10,5q�6j   $��5q�6y   �� 5      -   5        $��Share��5      ���	#FF1DB954
�������$
��10,5q�6d   $��5q�6s   5      �� 5   
   -   5         ��Name$ PlaylistListView��5       �� SelectionMode$ Single=�6-   $��1��6D   
������ $��
20,22,20,5q�6�   ��6Q   �� *Songs+��6�   Z� ��5      �� 5      .R�5      �� ���#191414$	��0,5q�62    �� Padding$ 10q�6?    �� CornerRadius$ 10��6L   ._�5      ]� .+�5      �� $
��Left=�6)   .+�5   "   �� 
�������$��18(�6Z   $	��5,0q�6h   ��6,   �� *	SongTitle+6h   �� 5    "   ���#CCCCCC$��14(�6]   $	��5,0q�6k   ��6,   �� *
ArtistName+6k   5!      �� 5"      $��Right=�6)   $ 
Horizontal=�6@   $��Right=�6Y   .+�5#   "   �� -   5        $
��Play��5#   )   ���	#FF1DB954
�������$
��10,5q�6v   $��5q�6�   �� 5$   "   -   5        $��Remove��5$   )   ���	#FF1DB954
�������$
��10,5q�6z   $��5q�6�   5%      5&      5'      5(      5*      5+      5,      