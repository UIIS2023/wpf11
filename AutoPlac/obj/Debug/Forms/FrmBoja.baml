   M S B A M L   `   `   `  ����C  ?AutoPlac, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:AutoPlac.FormsAutoPlac.Forms  5   	       AutoPlac.Forms.FrmBoja   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:AutoPlac.Forms     5   	     � Title$  Dodaj/Izmeni Boju��5   	   $	��260��6#   $	��350��60   ���#252A34 � 	Resources 5	         ButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property	 P� Value��%q  &	        �� 5
   
   " ��6$   .�5      ��  � �� �#FF2E636+   �� 5       � �� �#F7FBFC6+   �� 5       � ��$ 1q�60   �� 5       � �� �#FF2E636,   �� 5       � ��$ 13(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$	 100��6)   �� 5       � ��$ 30��6'   �� 5       � ��$ 0,3,5,0q�6'   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��6K   #��z��6w   $ 5��6�   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver	 . . �5   "   ��  � �� �#F7FBFC6?   �� 5   "    � �� �#FF2E636?   5      P� 5        � ��	 . . �5!   "   ��  �@J�Opacity$	 0.5Q�6<   5"      5$      5&      5(      5+      .��5,      � 
 � RowDefinitions
 5-   
   �� 5.      .+�51   
   �� ���#F7FBFC$��Naziv boje:��6%   $
��Left=�6;   $	��Top=�6V   $��	45,34,0,0q�6n   �� 52   
   -   5         ��Name$ txtNazivBoje��52      $	��120��6*   $
��Left=�66   $	��Top=�6Q   $��
164,40,0,0q�6i    �� TextWrapping$
 Wrap=�6}   �� 53   
   ���#F7FBFC$��Opis:��6%   $
��Left=�65   $	��Top=�6P   $��	47,67,0,0q�6h   �� 54   
   -   5        $ txtOpisBoje��54      $	��194��6(   $
��Left=�64   $	��Top=�6O   $��	90,73,0,0q�6g   $
 Wrap=�6z   $	��105��6�   �� 55   
   -   5        $��btnAdd��55      #��[  6   $	��Add��6D   $��80��6R   $��20��6]   $
��Left=�6i   $��Bottom=�6�   $��	26,0,0,10q�6�   6�   �� 56   
   -   5         ��Name$ 	btnCancel��56      #��[  6$   $��Cancel��6I   $��80��6Z   $��20��6e   $
��Left=�6q   $��Bottom=�6�   $��
135,0,0,10q�6�   6�   57      58      