   M S B A M L   `   `   `  ����C  ?AutoPlac, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:AutoPlac.FormsAutoPlac.Forms  5   	   !    AutoPlac.Forms.FrmDobavljac   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:AutoPlac.Forms     5   	     � Title$  Dodaj/Izmeni Dobavljaca��5   	   $	��250��6)   $	��350��66   ���#252A34 � 	Resources 5	         ButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property	 P� Value��%q  &	        �� 5
   
   " ��6$   .�5      ��  � �� �#FF2E636+   �� 5       � �� �#F7FBFC6+   �� 5       � ��$ 1q�60   �� 5       � �� �#FF2E636,   �� 5       � ��$ 13(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$	 100��6)   �� 5       � ��$ 30��6'   �� 5       � ��$ 0,3,5,0q�6'   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��6K   #��z��6w   $ 5��6�   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver	 . . �5   "   ��  � �� �#F7FBFC6?   �� 5   "    � �� �#FF2E636?   5      P� 5        � ��	 . . �5!   "   ��  �@J�Opacity$	 0.5Q�6<   5"      5$      5&      5(      5+      .��5,      � .+�5-   
   �� $��Naziv:��6   ���#F7FBFC$
��Left=�66   $��	42,44,0,0q�6Q   $	��Top=�6d   �� 5.   
   $��Kontakt:��6   ���#F7FBFC$
��Left=�68   $��	42,75,0,0q�6S   $	��Top=�6f   �� 5/   
   $��Adresa:��6   ���#F7FBFC$
��Left=�67   $��
42,106,0,0q�6R   $	��Top=�6f   �� 50   
   $��BrojRacuna:��6   ���#F7FBFC$
��Left=�6;   $��
42,137,0,0q�6V   $	��Top=�6j   �� 51   
   -   5        
 ��Name$
 txtNaziv��51      $
��Left=�6%   $��
122,48,0,0q�6@    �� TextWrapping$
 Wrap=�6T   $	��Top=�6h   $	��150��6�   �� 52   
   -   5        $
 
txtKontakt��52      $
��Left=�6&   $��
122,79,0,0q�6A   $
 Wrap=�6U   $	��Top=�6i   $	��150��6�   �� 53   
   -   5        $
 	txtAdresa��53      $
��Left=�6%   $��122,110,0,0q�6@   $
 Wrap=�6U   $	��Top=�6i   $	��150��6�   �� 54   
   -   5        $
 txtBrojRacuna��54      $
��Left=�6)   $��122,141,0,0q�6D   $
 Wrap=�6Y   $	��Top=�6m   $	��150��6�   �� 55   
   -   5        $��btnAdd��55      #��[  6   $	��Add��6D   $��80��6R   $��20��6]   $
��Left=�6i   $��Bottom=�6�   $��	26,0,0,10q�6�   6�   �� 56   
   -   5         ��Name$ 	btnCancel��56      #��[  6$   $��Cancel��6I   $��80��6Z   $��20��6e   $
��Left=�6q   $��Bottom=�6�   $��
135,0,0,10q�6�   6�   58      59      