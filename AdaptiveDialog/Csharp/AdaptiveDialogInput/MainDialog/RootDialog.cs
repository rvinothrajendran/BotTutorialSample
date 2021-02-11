using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Builder.LanguageGeneration;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Choices;


namespace AdaptiveDialogInput.MainDialog
{
    public sealed class RootDialog : AdaptiveDialog
    {
        private const string TextInput = "textinput";
        private const string NumberInput = "numberinput";
        private const string ConfirmInput = "confirminput";
        private const string ChoiceInput = "choiceinput";
        private const string DateTime = "datetime";
        private const string Attachment = "attachment";
        private const string LogIn = "login";
        private const string LogOut = "logout";

        private static OAuthInput _authentication;

        public RootDialog() : base(nameof(RootDialog))
        {

            string[] paths = { ".", "MainDialog", $"{nameof(RootDialog)}.lg" };
            var fullPath = Path.Combine(paths);

            Recognizer = CreateReconRecognizer();

            CreateAuthenticationInput();

            Triggers = new List<OnCondition>()
            {
                CreateUnknownIntent(),
                
                TextInputIntent(),
                
                NumberInputIntent(),
                
                ConfirmInputIntent(),
                
                ChoiceInputIntent(),
                
                DateTimeIntent(),
                
                AttachmentIntent(),

                CallLogIn(),
                
                CallLogOut()
                

            };
            

           Generator = new TemplateEngineLanguageGenerator(Templates.ParseFile(fullPath));
        }

        private static OnUnknownIntent CreateUnknownIntent()
        {
            return new OnUnknownIntent()
            {
                Actions = new List<Dialog>()
                {
                    new SendActivity("Unknown Intent")
                }
            };
        }

        private static OnIntent ConfirmInputIntent()
        {
            return new OnIntent()
            {
                Intent = ConfirmInput,
                Actions = new List<Dialog>()
                {
                    new ConfirmInput()
                    {
                        Prompt = new StaticActivityTemplate(MessageFactory.Text("Are you sure send your mobile number")),
                        Property = "user.confirm"
                    },
                        
                    new IfCondition()
                    {
                        Condition = "user.confirm == true",
                        Actions = new List<Dialog>()
                        {
                            MobileNumberInput(),
                            new SendActivity("Your mobile number is ${user.mobile}")
                        },
                        ElseActions = new List<Dialog>()
                        {
                            new SendActivity(MessageFactory.Text("Okay, Thank you"))
                        }
                    }
                }
            };
        }

        private static OnIntent NumberInputIntent()
        {
            return new OnIntent()
            {
                Intent = NumberInput,
                Actions = new List<Dialog>()
                {
                    MobileNumberInput(),
                    new SendActivity("Your mobile number is ${user.mobile}")
                }
            };
        }

        private static OnIntent TextInputIntent()
        {
            return new OnIntent()
            {
                Intent = TextInput,
                Actions = new List<Dialog>()
                {
                    new TextInput()
                    {
                        Property = "user.name",
                        Prompt = new StaticActivityTemplate(MessageFactory.Text("Hey enter the name ")),
                        
                    },

                    new SendActivity("Hello , ${user.name} Thank you"),
                    
                }
            };
        }


        private static OnIntent ChoiceInputIntent()
        {
            return new OnIntent()
            {
                Intent = ChoiceInput,
                Actions = new List<Dialog>()
                {
                    new ChoiceInput()
                    {
                        Prompt = new ActivityTemplate("Please select the mobile type"),
                        Choices = new ChoiceSet(new List<Choice>()
                        {
                            new Choice("IPhone"),
                            new Choice("Surface Duo"),
                            new Choice("Android")
                        }),
                        Property = "user.device",
                        Style = ListStyle.HeroCard,
                        DefaultValue = "No Device",
                        MaxTurnCount = 1
                    },
                    
                    new SendActivity("Thank you , you have selected mobile ${user.device}"),
                    
                    new SwitchCondition()
                    {
                        Condition = "user.device",
                        Cases = new List<Case>()
                        {
                            new Case()
                            {
                                Value = "IPhone",
                                Actions = new List<Dialog>()
                                {
                                    new SendActivity("${user.device} from Apple")
                                }
                            },
                            new Case()
                            {
                                Value = "Surface Duo",
                                Actions = new List<Dialog>()
                                {
                                    new SendActivity("${user.device} from Microsoft")
                                }
                            },
                            new Case()
                            {
                                Value = "Android",
                                Actions = new List<Dialog>()
                                {
                                    new SendActivity("${user.device} from Google")
                                }
                            }
                        },
                        Default = new List<Dialog>()
                        {
                            new SendActivity("Sorry , the product not in your list")
                        }
                    }
                }
            };
        }


        private static OnIntent DateTimeIntent()
        {
            return new OnIntent()
            {
                Intent = DateTime,
                
                Actions = new List<Dialog>()
                {
                    new DateTimeInput()
                    {
                        Prompt = new StaticActivityTemplate(MessageFactory.Text("When is the conference")),
                        Property = "user.conference"
                    },
                    
                    new SendActivity("${user.conference}"),
                    new SendActivity("Value : ${user.conference[0].Value}")
                }
                
               
            };
        }


        private static OnIntent AttachmentIntent()
        {
            return new OnIntent()
            {
                Intent = Attachment,
                Actions = new List<Dialog>()
                {
                   new AttachmentInput()
                   {
                       Prompt = new StaticActivityTemplate(MessageFactory.Text("Please upload the images")),
                       Property = "user.images",
                       OutputFormat = AttachmentOutputFormat.First,
                   },
                   
                   new SendActivity("${user.images}"),
                   
                   new SaveFiles()
                   {
                       FilesInfo = "${user.images}"
                   }

                }
            };
        }


        private static void CreateAuthenticationInput()
        {
            _authentication = new OAuthInput()
            {
                ConnectionName = "",
                Title = "Sample of Auth",
                Text = "Please sign in",
                Property = "turn.auth",
                Timeout = 4 * 60000
            };
        }

        private static OnIntent CallLogIn()
        {
            return new OnIntent()
            {
                Intent = LogIn,
                Actions = new List<Dialog>()
                {
                    _authentication,
                    new IfCondition()
                    {
                        Condition = "turn.auth.token && length(turn.auth.token)",
                        Actions = new List<Dialog>()
                        {
                            new SendActivity("Login in success")
                        },
                        ElseActions = new List<Dialog>()
                        {
                            new SendActivity("Sorry login failed")
                        }
                    }
                    
                }
                
            };
        }

        private static OnIntent CallLogOut()
        {
            return new OnIntent()
            {
                Intent = LogOut,
                Actions = new List<Dialog>()
                {
                    new CodeAction(CallFunc)
                }

            };
        }

        private static async Task<DialogTurnResult> CallFunc(DialogContext dc, object obj)
        {
            var item = await _authentication.GetUserTokenAsync(dc,new CancellationToken());
            await _authentication.SignOutUserAsync(dc, new CancellationToken());
            await dc.Context.SendActivityAsync(MessageFactory.Text("Sign out success"), new CancellationToken());
            return new DialogTurnResult(DialogTurnStatus.Complete);
        }

        private static RegexRecognizer CreateReconRecognizer()
        {
            var recognizer = new RegexRecognizer()
            {
                Intents = new List<IntentPattern>()
                {
                    new IntentPattern(TextInput, "(?i)textinput"),
                    new IntentPattern(NumberInput,"(?i)numberinput"),
                    new IntentPattern(ConfirmInput,"(?i)confirminput"),
                    new IntentPattern(ChoiceInput,"(?i)choiceinput"),
                    new IntentPattern(DateTime,"(?i)datetime"),
                    new IntentPattern(Attachment,"(?i)attachment"),
                    new IntentPattern(LogIn,"(?i)login"),
                    new IntentPattern(LogOut,"(?i)logout")
                },
            };

            return recognizer;
        }

        private static NumberInput MobileNumberInput()
        {
            return new NumberInput()
            {
                Id = "numberId",
                Prompt = new StaticActivityTemplate(MessageFactory.Text("Please enter the mobile number123")),
                Property = "user.mobile",
                Validations = new List<BoolExpression>()
                {
                    "length(string(this.value)) == 10"
                },
                MaxTurnCount = 3,
                DefaultValue = "1234567890",
                DefaultValueResponse = new ActivityTemplate("Hey , I sent the Default value ${%DefaultValue}"),
                UnrecognizedPrompt = new ActivityTemplate("Please enter only number"),
                InvalidPrompt = new ActivityTemplate("Please enter mobile number with 10 digit")
            };
        }


    }
}